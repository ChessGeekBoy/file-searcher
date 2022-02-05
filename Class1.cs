namespace System.FileManager
{
    /// <summary>
    /// Class to manage file searching
    /// </summary>
    /// 
    public static class Searcher
    {
        /// <summary>
        /// A method to search for a directory or file recursively and asynchronously
        /// </summary>
        /// <param name="startingDirectory">The starting directory of the search (defaults to "C:")</param>
        /// <param name="query">The string being searched for</param>
        /// <returns>Results of searching for a file/directory asynchronously</returns>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public static async Task<List<string>> SearchAsync(string startingDirectory, string query)
        {
            // Check if the starting directory exists; if not, throw a DirectoryNotFound exception
            if (!Directory.Exists(startingDirectory))
            {
                throw new DirectoryNotFoundException($"{startingDirectory} is not a valid directory");
            }

            // Initializing variables
            DirectoryInfo startingDirectoryInfo = new DirectoryInfo(startingDirectory);
            List<string> results = new List<string>();

            // Going through each child directory, searching it and adding the results to a list
            foreach (DirectoryInfo directoryInfo in startingDirectoryInfo.EnumerateDirectories())
            {
                switch (directoryInfo.Name.ToLowerInvariant().Contains(query.ToLowerInvariant()))
                {
                    case true:
                        results.Add(directoryInfo.FullName);
                        break;
                    default:
                        break;
                }

                // Using Task.Factory.StartNew() to search asynchronously
                List<string> directoryResults = await
                    Task.Factory.StartNew(
                        () => SearchAsync(directoryInfo.FullName, query)
                        ).Result;

                // Adding the searches results to the "results" variable
                results.AddRange(directoryResults);
            }

            // Searching through the files in each directory
            foreach (FileInfo file in startingDirectoryInfo.EnumerateFiles())
            {
                switch (file.Name.ToLowerInvariant() == query.ToLowerInvariant())
                {
                    case true:
                        results.Add(file.FullName);
                        break;
                    default:
                        break;
                }
            }

            return results;
        }
    }
}