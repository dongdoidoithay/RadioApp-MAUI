﻿using IImage = Microsoft.Maui.IImage;

namespace RadioApp.Handlers.GaussianImage;
/// <summary>
/// 图像高斯模糊接口
/// </summary>
public interface IGaussianImage : IImage
{
    byte[] SourceByteArray { get; set; }
}