/*
 * Copyright (c) 2022. Gavin Pease.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
 * associated documentation files (the "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
 * of the Software, and to permit persons to whom the Software is furnished to do so, subject to the
 * following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or substantial
 * portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
 * LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT
 * OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlogSite.Interfaces;

namespace BlogSite.Models;

public class Posts : IPostModel
{
    [Key]
    [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// The title, without any script tags.
    /// </summary>
    private string _title = string.Empty;

    /// <summary>
    /// Whether this post is a draft
    /// </summary>
    [Required]
    [Display(Name = "Draft?")]
    public bool IsDraft { get; set; }

    [Required]
    [Display(Name = "Title")]
    [StringLength(80)]
    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            if (value == null)
                _title = "";
            _title = _title.Replace("script", "");
            _title = _title.Replace("onload", "");
            _title = _title.Replace("onerror", "");
        }
    }

    /// <summary>
    /// The content without any script tags.
    /// </summary>
    private string _content = string.Empty;

    [Required]
    [Display(Name = "Content")]
    public string Content
    {
        get => _content;
        set
        {
            _content = value;
            if (value == null)
                _content = "";
            _content = _content.Replace("script", "");
            _title = _title.Replace("onload", "");
            _title = _title.Replace("onerror", "");
        }
    }

    public string AuthorId { get; set; }

    [Display(Name = "Author")]
    [ForeignKey("AuthorId")]
    public Poster Author { get; set; }
}