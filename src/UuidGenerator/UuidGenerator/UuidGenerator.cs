// BAADER Code Cleanup
using System.Security.Cryptography;

namespace UuidGenerator;

/// <summary>
/// A utility class for generating UUIDs.
/// </summary>
public static class UuidGenerator
{
  /// <summary>
  /// Generates a new UUIDv7 based on the current UTC time.
  /// </summary>
  /// <returns>A new UUIDv7 <see cref="Guid"/>.</returns>
  public static Guid NewUuid7() => NewUuid7(DateTimeOffset.UtcNow);
  
  /// <summary>
  /// Generates a new UUIDv7 based on the specified date and time.
  /// </summary>
  /// <param name="dateTimeOffset">The date and time to base the UUIDv7 on.</param>
  /// <returns>A new UUIDv7 <see cref="Guid"/>.</returns>
  public static Guid NewUuid7(DateTimeOffset dateTimeOffset)
  {
    Span<byte> uuidAsBytes = stackalloc byte[16];
    FillTimePart(uuidAsBytes, dateTimeOffset);
    var random_part = uuidAsBytes[6..];
    RandomNumberGenerator.Fill(random_part);

    //Set Version 0x7
    uuidAsBytes[6] &= 0x0F;
    uuidAsBytes[6] |= 0x70;

    //Set variant 0x8 (RFC 9562)
    uuidAsBytes[8] &= 0x3F;
    uuidAsBytes[8] |= 0x80;
    
    return new Guid(uuidAsBytes, true);
  }
  
  /// <summary>
  /// Fills the first part of the UUID with the timestamp based on the specified date and time.
  /// </summary>
  /// <param name="uuidAsBytes">The byte span representing the UUID.</param>
  /// <param name="dateTimeOffset">The date and time to base the UUIDv7 on.</param>
  private static void FillTimePart(Span<byte> uuidAsBytes, DateTimeOffset dateTimeOffset)
  {
    long currentTimestamp = dateTimeOffset.ToUnixTimeMilliseconds();

    Span<byte> current = stackalloc byte[8];
    BitConverter.TryWriteBytes(current, currentTimestamp);
    //var current = BitConverter.GetBytes(currentTimestamp).AsSpan();
    if (BitConverter.IsLittleEndian)
    {
      current.Reverse();
    }
    current[2..8].CopyTo(uuidAsBytes);
  }

  /// <summary>
  /// Generates a new UUIDv4.
  /// </summary>
  /// <returns>A new UUIDv4 <see cref="Guid"/>.</returns>
  public static Guid NewUuid4() => Guid.NewGuid();
}