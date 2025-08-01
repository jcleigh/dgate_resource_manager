using System;
using System.Collections.Generic;
using System.IO;

namespace DGateResourceManager.Services;

/// <summary>
/// Specialized parser for Death Gate text resources that handles the game's proprietary
/// text compression formats. Death Gate used sophisticated Huffman compression algorithms
/// similar to those found in other Legend Entertainment games.
/// 
/// This implementation provides basic parsing capabilities and serves as a foundation
/// for more advanced decompression algorithms found in the original C++ version.
/// </summary>
public class TextParser
{
    /// <summary>
    /// Header structure for Death Gate text resources, matching the cHeader structure
    /// from the original C++ implementation. Contains metadata about the compressed text data.
    /// </summary>
    public struct TextHeader
    {
        /// <summary>Number of text strings contained in this resource</summary>
        public ushort StringCount;
        
        /// <summary>Size of the compressed data stream in bytes</summary>
        public ushort StreamSize;
        
        /// <summary>Compression flags indicating format version and options</summary>
        public ushort Flags;
    }

    /// <summary>
    /// Parse text resource data. This is a simplified implementation.
    /// The original C++ version had complex Huffman decompression with dictionary support.
    /// </summary>
    public static List<string> ParseTextData(byte[] data)
    {
        var textLines = new List<string>();
        
        if (data.Length < 6) // Minimum size for header
        {
            textLines.Add("Invalid data: too small for header");
            return textLines;
        }

        try
        {
            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            // Read header (similar to original cHeader struct)
            var header = new TextHeader
            {
                StringCount = reader.ReadUInt16(),
                StreamSize = reader.ReadUInt16(),
                Flags = reader.ReadUInt16()
            };

            textLines.Add($"Text Resource Header:");
            textLines.Add($"  String Count: {header.StringCount}");
            textLines.Add($"  Stream Size: {header.StreamSize}");
            textLines.Add($"  Flags: 0x{header.Flags:X4}");
            textLines.Add("");

            // Check if we have compressed data (similar to original version detection)
            if (header.StreamSize > 0 && header.StreamSize < data.Length - 6)
            {
                textLines.Add("Compressed text data detected.");
                textLines.Add("Note: Full Huffman decompression not yet implemented.");
                textLines.Add("Original C++ implementation used complex dictionary-based decompression.");
                textLines.Add("");
                
                // Try to extract some readable strings (simple approach)
                var remainingData = new byte[data.Length - 6];
                Array.Copy(data, 6, remainingData, 0, remainingData.Length);
                
                var readableStrings = ExtractReadableStrings(remainingData);
                if (readableStrings.Count > 0)
                {
                    textLines.Add("Potentially readable strings found:");
                    textLines.AddRange(readableStrings);
                }
            }
            else
            {
                // Try to read as plain text
                var remainingData = new byte[data.Length - 6];
                Array.Copy(data, 6, remainingData, 0, remainingData.Length);
                
                var text = System.Text.Encoding.ASCII.GetString(remainingData);
                var lines = text.Split(new[] { '\r', '\n', '\0' }, StringSplitOptions.RemoveEmptyEntries);
                
                textLines.Add("Plain text content:");
                textLines.AddRange(lines);
            }
        }
        catch (Exception ex)
        {
            textLines.Add($"Error parsing text data: {ex.Message}");
        }

        return textLines;
    }

    /// <summary>
    /// Extract potentially readable strings from binary data
    /// </summary>
    private static List<string> ExtractReadableStrings(byte[] data)
    {
        var strings = new List<string>();
        var currentString = new List<byte>();
        
        foreach (byte b in data)
        {
            if (b >= 32 && b <= 126) // Printable ASCII
            {
                currentString.Add(b);
            }
            else
            {
                if (currentString.Count >= 3) // Minimum length for meaningful string
                {
                    var str = System.Text.Encoding.ASCII.GetString(currentString.ToArray()).Trim();
                    if (!string.IsNullOrWhiteSpace(str))
                    {
                        strings.Add($"  \"{str}\"");
                    }
                }
                currentString.Clear();
            }
        }
        
        // Check final string
        if (currentString.Count >= 3)
        {
            var str = System.Text.Encoding.ASCII.GetString(currentString.ToArray()).Trim();
            if (!string.IsNullOrWhiteSpace(str))
            {
                strings.Add($"  \"{str}\"");
            }
        }
        
        return strings;
    }

    /// <summary>
    /// Placeholder for the complex Huffman decompression from the original C++.
    /// The original implementation included:
    /// - Huffman tree parsing
    /// - Dictionary-based decompression
    /// - Bit-level stream processing
    /// - Multiple compression versions (2, 3, 4)
    /// </summary>
    public static byte[] DecompressHuffman(byte[] compressedData, TextHeader header)
    {
        // This would implement the decomp() function from the original C++
        // For now, return the input data unchanged
        throw new NotImplementedException("Huffman decompression requires complex bit-level processing from the original C++ implementation");
    }
}