using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using TestHelperExtensions;
using PPTail.Interfaces;

namespace PPTail.Generator.Encoder.Test
{
    public class ContentEncoder_URLEncode_Should
    {
        [Theory]
        [InlineData("", "")]
        [InlineData("   ", "")]
        [InlineData("My Title", "My-Title")]
        [InlineData(" AnotherTitle", "AnotherTitle")]
        [InlineData("AnotherTitle ", "AnotherTitle")]
        [InlineData(" More Titles ", "More-Titles")]
        [InlineData("My Little Title ", "My-Little-Title")]
        [InlineData("Title with &quot; code", "Title-with-code")]
        [InlineData("Title with \" character", "Title-with-character")]
        [InlineData("John Doe's possesive", "John-Does-possesive")]
        [InlineData("less than &lt; symbol", "less-than-symbol")]
        [InlineData("greater than &gt; symbol", "greater-than-symbol")]
        [InlineData("&lt;html&gt; tag", "html-tag")]
        [InlineData("<html> tag", "html-tag")]
        [InlineData("Question mark?", "Question-mark")]
        [InlineData("Will &quot;Augmented Reality&quot; Finally Make My Life's Dream Come True?", "Will-Augmented-Reality-Finally-Make-My-Lifes-Dream-Come-True")]
        [InlineData("Yeah! Awesome!", "Yeahbang-Awesomebang")]
        [InlineData("Yeah----Awesome---Lot's of     spaces  ", "Yeah-Awesome-Lots-of-spaces")]
        [InlineData("Includes MSWord “smartquotes”", "Includes-MSWord-smartquotes")]
        [InlineData("Handles an en–dash", "Handles-an-en-dash")]
        [InlineData("Converts an encoded ene28093dash to a normal dash", "Converts-an-encoded-en-dash-to-a-normal-dash")]
        [InlineData("Removes encoded e2809csmartquotese2809d", "Removes-encoded-smartquotes")]
        [InlineData("Removes.Dots", "RemovesdotDots")]
        [InlineData("Removes. all.net", "Removesdot-alldotnet")]
        public void ProperlyEncodeTheString(String source, String expected)
        {
            var container = (null as IServiceCollection).Create();
            var target = new ContentEncoder(container.BuildServiceProvider());
            Assert.Equal(expected, target.UrlEncode(source));
        }

    }
}
