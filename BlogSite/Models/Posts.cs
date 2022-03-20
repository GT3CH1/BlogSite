﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlogSite.Data;
using BlogSite.Interfaces;

namespace BlogSite.Models;

public class Posts : IPostModel
{
    [Key] [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]  
    public int Id { get; set; }

    /// <summary>
    /// The title, without any script tags.
    /// </summary>
    private string _title = string.Empty;

    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            _title = _title.Replace("script", "");
            _title = _title.Replace("onload", "");
            _title = _title.Replace("onerror", "");
        }
    }

    /// <summary>
    /// The content without any script tags.
    /// </summary>
    private string _content = string.Empty;

    public string Content
    {
        get => _content;
        set
        {
            _content = value;
            _content = _content.Replace("script", "");
            _title = _title.Replace("onload", "");
            _title = _title.Replace("onerror", "");
        }
    }

    public Posts(string title, string content, int id)
    {
        Title = title;
        Content = content;
        Id = id;
    }

    public Posts(string title, string content)
    {
        Title = title;
        Content = content;
    }

    public Posts()
    {
    }
}