using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.SiteGenerator;
using Xunit;

namespace PPTail.SiteGenerator.Test
{
    public class String_CreateSlug_Should
    {
        [Theory]
        [InlineData("", "")]
        [InlineData("   ", "")]
        [InlineData("My Title","My-Title")]
        [InlineData(" AnotherTitle", "AnotherTitle")]
        [InlineData("AnotherTitle ", "AnotherTitle")]
        [InlineData(" More Titles ", "More-Titles")]
        [InlineData("My Little Title ", "My-Little-Title")]
        [InlineData("Title with &quot; code","Title-with-code")]
        [InlineData("Title with \" character", "Title-with-character")]
        [InlineData("John Doe's possesive", "John-Does-possesive")]
        [InlineData("less than &lt; symbol", "less-than-symbol")]
        [InlineData("greater than &gt; symbol", "greater-than-symbol")]
        [InlineData("&lt;html&gt; tag", "html-tag")]
        [InlineData("<html> tag", "html-tag")]
        [InlineData("Question mark?", "Question-mark")]
        [InlineData("Will &quot;Augmented Reality&quot; Finally Make My Life's Dream Come True?", "Will-Augmented-Reality-Finally-Make-My-Lifes-Dream-Come-True")]
        [InlineData("Yeah! Awesome!", "Yeah-Awesome")]
        [InlineData("Yeah----Awesome---Lot's of     spaces  ", "Yeah-Awesome-Lots-of-spaces")]
        [InlineData("Includes MSWord “smartquotes”", "Includes-MSWord-smartquotes")]
        [InlineData("Handles an en–dash", "Handles-an-en-dash")]
        [InlineData("Converts an encoded ene28093dash to a normal dash", "Converts-an-encoded-en-dash-to-a-normal-dash")]
        [InlineData("Removes encoded e2809csmartquotese2809d", "Removes-encoded-smartquotes")]
        public void ProperlyEncodeTheString(string source, string expected)
        {
            Assert.Equal(expected, source.CreateSlug());
        }
    }
}
