﻿
namespace RadioApp.Handlers.GaussianImage;

/// <summary>
/// Image Gauss
/// </summary>
public class GaussianImage : Image, IGaussianImage
{
    public static readonly BindableProperty SourceByteArrayProperty =
        BindableProperty.Create(
            nameof(SourceByteArray),
            typeof(byte[]),
            typeof(GaussianImage),
            null);
    public byte[] SourceByteArray
    {
        get { return (byte[])GetValue(SourceByteArrayProperty); }
        set { SetValue(SourceByteArrayProperty, value); }
    }
}