using System;
using Xunit;

namespace PPTail.Data.Forestry.Test;

public class StringExtensions_GetRelativePathFrom_Should
{
    [Theory]
    [InlineData("C:\\Root\\Folder\\File.txt", "C:\\Root", "Folder")]
    [InlineData("C:\\File.txt", "C:\\", "")]
    [InlineData("c:\\b8870719\\16bd8553\\5f2b1087\\234973.md", "c:\\b8870719\\16bd8553", "5f2b1087")]
    [InlineData("\\Folder\\File.txt", "\\Folder", "")]
    [InlineData("\\File.txt", "\\", "")]
    [InlineData("..\\..\\MySiteRoot\\build-site.sh", "..\\..\\MySiteRoot", "")]
    public void ReturnTheProperRelativePath(string fileLocation, string containingFolderPath, string expected)
    {
        var actual = fileLocation.GetRelativePathFrom(containingFolderPath);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("File.txt", "\\RandomFolder")]
    [InlineData("..\\Folder\\File.txt", "RandomFolder")]
    public void ThrowIfNoRelativePathCanBeDetermined(string source, string root)
    {
        Assert.Throws<ArgumentException>(() => source.GetRelativePathFrom(root));
    }

}
