﻿using Lumina.Data.Parsing.Layer;
using System;
using System.Collections.Generic;
using Umbra.Common;
using Una.Drawing;

namespace Umbra.Widgets;

public class ContextMenu
{
    public Action<ContextMenuEntry>? OnEntryInvoked;

    public string Id           { get; }              = Guid.NewGuid().ToString();
    public bool   ShouldRender { get; private set; } = false;

    private readonly Dictionary<string, ContextMenuEntry> _entries = [];

    internal Node Node = new() {
        Id         = "ContextMenu",
        ClassList  = ["context-menu"],
        Stylesheet = Stylesheet,
        ChildNodes = [],
    };

    public ContextMenu(IEnumerable<ContextMenuEntry> entries)
    {
        Node.Id = $"CM_{Id}";

        foreach (var entry in entries) {
            Node.ChildNodes.Add(entry.Node);
            var e = entry;
            entry.OnInvoke += () => OnEntryInvoked?.Invoke(e);
            _entries.TryAdd(entry.Id, entry);
        }
    }

    public ContextMenuEntry? GetEntry(string id)
    {
        return _entries.GetValueOrDefault(id);
    }

    public void SetEntryVisible(string id, bool isVisible)
    {
        var entry = GetEntry(id);
        if (entry != null) {
            entry.IsVisible = isVisible;
        }
    }

    public void SetEntryDisabled(string id, bool isDisabled)
    {
        var entry = GetEntry(id);
        if (entry != null) {
            entry.IsDisabled = isDisabled;
        }
    }

    public void Present()
    {
        Framework.Service<ContextMenuManager>().Present(this);
        ShouldRender = true;
    }

    private static Stylesheet Stylesheet { get; } = new(
        [
            new(
                ".context-menu",
                new() {
                    Flow            = Flow.Vertical,
                    Padding         = new(8),
                    BackgroundColor = new("Widget.PopupBackground"),
                    StrokeColor     = new("Widget.PopupBorder"),
                    StrokeWidth     = 1,
                    StrokeInset     = 1,
                    ShadowSize      = new(32),
                    ShadowInset     = 4,
                    Gap             = 4,
                    BorderRadius    = 8,
                    IsAntialiased   = false,
                }
            ),
        ]
    );
}
