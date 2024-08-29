﻿namespace RadioApp.HandCursorControls;
/// <summary>
/// Hand cursor
/// </summary>
internal class HandCursor
{
    private HandCursor()
    {

    }

    /// <summary>
    /// Binded the cursor for components
    /// </summary>
    public static void Binding()
    {
#if WINDOWS
        //Microsoft.Maui.Handlers.BorderHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
        //{
        //    if (view is HandCursorBorder)
        //    {
        //        handler.PlatformView.PointerEntered += (_, __) => handler.PlatformView.SetHandCursor();

        //    }
        //});
        //Microsoft.Maui.Handlers.ButtonHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
        //{
        //    if (view is HandCursorButton)
        //    {
        //        handler.PlatformView.PointerEntered += (_, __) => handler.PlatformView.SetHandCursor();

        //    }
        //});
        //Microsoft.Maui.Handlers.LayoutHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
        //{
        //    if (view is HandCursorStackLayout)
        //    {
        //        handler.PlatformView.PointerEntered += (_, __) => handler.PlatformView.SetHandCursor();

        //    }
        //});
        //Microsoft.Maui.Handlers.ImageHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
        //{
        //    if (view is HandCursorImage)
        //    {
        //        handler.PlatformView.PointerEntered += (_, __) => handler.PlatformView.SetHandCursor();

        //    }
        //});
        //Microsoft.Maui.Handlers.LabelHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
        //{
        //    if (view is HandCursorLabel)
        //    {
        //        handler.PlatformView.PointerEntered += (_, __) => handler.PlatformView.SetHandCursor();

        //    }
        //});
#endif
    }
}