using PPTail.Interfaces;
using PPTail.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace PPTConvert
{
    public static class StringExtensions
    {
        const string _connectionStringProviderKey = "Provider";
        const string _connectionStringFilePathKey = "FilePath";

        public static IContentRepository GetSourceRepository(this string connectionString, IServiceProvider serviceProvider)
        {
            var providerName = connectionString.GetConnectionStringValue(_connectionStringProviderKey);
            
            IContentRepository result = null;
            switch (providerName)
            {
                case "PPTail.Data.Ef.Repository":
                    result = new PPTail.Data.Ef.Repository(serviceProvider);
                    break;

                case "PPTail.Data.FileSystem.Repository":
                    result = new PPTail.Data.FileSystem.Repository(serviceProvider, connectionString);
                    break;

                case "PPTail.Data.Forestry.Repository":
                    result = new PPTail.Data.Forestry.Repository(serviceProvider, connectionString);
                    break;

                case "PPTail.Data.MediaBlog.Repository":
                    result = new PPTail.Data.MediaBlog.Repository(serviceProvider, connectionString);
                    break;

                case "PPTail.Data.MediaBlog.YamlRepository":
                    result = new PPTail.Data.MediaBlog.YamlRepository(serviceProvider, connectionString);
                    break;

                case "PPTail.Data.NativeJson.Repository":
                    result = new PPTail.Data.NativeJson.Repository(serviceProvider, connectionString);
                    break;

                case "PPTail.Data.PhotoBlog.Repository":
                    result = new PPTail.Data.PhotoBlog.Repository(serviceProvider, connectionString);
                    break;

                case "PPTail.Data.WordpressFiles.Repository":
                    result = new PPTail.Data.WordpressFiles.Repository(serviceProvider, connectionString);
                    break;

                default:
                    throw new ArgumentException($"Provider '{providerName}' not found.");
            }

            return result;
        }

        public static IContentRepositoryWriter GetTargetRepository(this string connectionString, IServiceProvider serviceProvider)
        {
            var providerName = connectionString.GetConnectionStringValue(_connectionStringProviderKey);
            string filePath = connectionString.GetConnectionStringValue(_connectionStringFilePathKey);

            IContentRepositoryWriter result = null;
            switch (providerName)
            {
                case "PPTail.Data.NativeJson.RepositoryWriter":
                    result = new PPTail.Data.NativeJson.RepositoryWriter(filePath);
                    break;

                case "PPTail.Data.Forestry.RepositoryWriter":
                    result = new PPTail.Data.Forestry.RepositoryWriter(serviceProvider, filePath, providerName);
                    break;

                default:
                    throw new ArgumentException($"Provider '{providerName}' not found.");
            }

            return result;
        }

        public static (String sourceConnection, String targetConnection) ParseArguments(this string[] args)
        {
            return args is null ? ((String sourceConnection, String targetConnection))(null, null) : (args[0], args[1]);
        }

        public static (bool argsAreValid, IEnumerable<string> argumentErrors) ValidateArguments(this string[] args)
        {
            const Int32 expectedArgCount = 2;

            var errors = new List<string>();
            bool isValid = ((args?.Length == expectedArgCount) && !args.IsNullOrWhiteSpace());

            if ((args is null) || (args.Length != expectedArgCount))
                errors.Add("Usage - PPTConvert.exe SourceConnectionString TargetConnectionString");
            else
            {
                if (string.IsNullOrEmpty(args[0]))
                    errors.Add("A value must be supplied for the SourceConnectionString argument");

                if (string.IsNullOrEmpty(args[1]))
                    errors.Add("A value must be supplied for the TargetConnectionString argument");
            }

            return (isValid, errors);
        }


        /// <summary>
        /// Checks to see if any of the supplied values are null or empty whitespace
        /// </summary>
        /// <param name="args">A String array containing the strings to be checked for nullness</param>
        /// <returns>Returns TRUE if ANY of the supplied values are null or empty whitespace, FALSE if
        /// all contain values.  Returns FALSE if the argument array is empty or null since none of
        /// it's containing values are then null or whitespace (since there are none).</returns>
        public static bool IsNullOrWhiteSpace(this string[] args)
        {
            bool result = false;
            if (!(args is null))
                foreach (var arg in args)
                    result = (result || string.IsNullOrWhiteSpace(arg));
            return result;
        }
    }
}
