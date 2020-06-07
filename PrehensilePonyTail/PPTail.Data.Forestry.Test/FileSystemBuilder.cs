using Moq;
using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestHelperExtensions;

namespace PPTail.Data.Forestry.Test
{
    public class FileSystemBuilder
    {
        private readonly List<(string FileName, string Content)> _contentItemFiles = new List<(string, string)>();
        private readonly CategoriesFileBuilder _categoriesFileBuilder = new CategoriesFileBuilder();

        public IFile Build()
        {
            var fileSystem = new Mock<IFile>();

            foreach (var (fileName, content) in _contentItemFiles)
            {
                fileSystem.Setup(f => f.ReadAllText(It.Is<String>(s => s.EndsWith(fileName))))
                    .Returns(content);
            }

            fileSystem.Setup(f => f.ReadAllText(It.Is<String>(s => s.EndsWith("Categories.md"))))
                .Returns(_categoriesFileBuilder.Build());

            return fileSystem.Object;
        }

        public IEnumerable<String> ContentItemFileNames => _contentItemFiles.Select(f => f.FileName);

        public FileSystemBuilder AddCategories(IEnumerable<Category> categories)
        {
            _categoriesFileBuilder.AddCategories(categories);
            return this;
        }

        public FileSystemBuilder AddRandomCategories()
        {
            _categoriesFileBuilder.AddRandomCategories();
            return this;
        }

        public FileSystemBuilder AddRandomCategories(int count)
        {
            _categoriesFileBuilder.AddRandomCategories(count);
            return this;
        }

        public FileSystemBuilder AddRandomCategory()
        {
            _categoriesFileBuilder.AddRandomCategory();
            return this;
        }

        public FileSystemBuilder AddContentItemFileWithRandomContent(string fileName)
        {
            string content = new ContentItemFileBuilder().UseRandomValues().Build();
            return this.AddContentItemFile(fileName, content);
        }

        public FileSystemBuilder AddContentItemFile(string fileName, string content)
        {
            _contentItemFiles.Add((fileName, content));
            return this;
        }

        public FileSystemBuilder AddRandomContentItemFile()
        {
            string fileName = $"{string.Empty.GetRandom()}.md";
            string content = new ContentItemFileBuilder().UseRandomValues().Build();
            _contentItemFiles.Add((fileName, content));
            return this;
        }

        public FileSystemBuilder AddRandomContentItemFiles()
        {
            return this.AddRandomContentItemFiles(20.GetRandom(3));
        }

        public FileSystemBuilder AddRandomContentItemFiles(int count)
        {
            for (int i = 0; i < count; i++)
                this.AddRandomContentItemFile();
            return this;
        }
    }
}
