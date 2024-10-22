// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ElevenLabs.TextToSpeech;

public sealed class TextToSpeechWebSocketRequest
{
    /// <summary>
    ///     Text needs to end with a space and cannot be null or empty.
    /// </summary>
    /// <param name="text">The text to be converted to speech. Needs to end with a space, cannot be null or empty.</param>
    /// <param name="flush">
    ///     Forces the generation of audio. Set this value to true when you have finished sending text, but
    ///     want to keep the websocket connection open.
    /// </param>
    /// <param name="tryTriggerGeneration">
    ///     Use this to attempt to immediately trigger the generation of audio. Most users
    ///     shouldn't use this.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="text" /> is null or empty.</exception>
    public TextToSpeechWebSocketRequest(string text, bool? flush = null, bool tryTriggerGeneration = false)
    {
        // if the last character of the text is not a space, append one
        Text = text[^1] != ' ' ? text + ' ' : text;
        TryTriggerGeneration = tryTriggerGeneration;
        Flush = flush;
    }

    /// <summary>
    ///     The text to be converted to speech. The last character of the text must be a space.
    /// </summary>
    [JsonPropertyName("text"), JsonInclude]
    public string Text { get; }

    /// <summary>
    ///     Use this to attempt to immediately trigger the generation of audio. Most users shouldn't use this.
    /// </summary>
    [JsonPropertyName("try_trigger_generation")]
    public bool TryTriggerGeneration { get; }

    /// <summary>
    ///     Flush forces the generation of audio. Set this value to true when you have finished sending text,
    ///     but want to keep the websocket connection open.
    /// </summary>
    [JsonPropertyName("flush"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Flush { get; }

    public ArraySegment<byte> ToArraySegment()
    {
        string json = JsonSerializer.Serialize(this);
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        return new ArraySegment<byte>(bytes);
    }
}