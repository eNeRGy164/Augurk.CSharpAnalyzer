/*
 Copyright 2017, Augurk
 
 Licensed under the Apache License, Version 2.0 (the "License");
 you may not use this file except in compliance with the License.
 You may obtain a copy of the License at
 
 http://www.apache.org/licenses/LICENSE-2.0
 
 Unless required by applicable law or agreed to in writing, software
 distributed under the License is distributed on an "AS IS" BASIS,
 WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 See the License for the specific language governing permissions and
 limitations under the License.
*/
using Augurk.CSharpAnalyzer.Options;
using Oakton;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Augurk.CSharpAnalyzer.Commands
{
    /// <summary>
    /// An <see cref="OaktonAsyncCommand{T}"/> that implements uploading analysis results to Augurk.
    /// </summary>
    [Description("Upload the results of an analysis to Augurk")]
    public class UploadCommand : OaktonAsyncCommand<UploadOptions>
    {
        public async override Task<bool> Execute(UploadOptions input)
        {
            // Check if the provided solution path is rooted
            string solutionFile = input.Solution;
            if (!Path.IsPathRooted(solutionFile))
            {
                solutionFile = Path.Combine(Environment.CurrentDirectory, solutionFile);
            }

            // Check to make sure that the solution file exists
            if (!File.Exists(solutionFile))
            {
                ConsoleWriter.Write(ConsoleColor.Red, $"Solution file '{solutionFile}' does not exist.");
                return false;
            }

            // Check to make sure an associated analysis results file exists
            string analysisResultsFileName = Path.Combine(Path.GetDirectoryName(solutionFile), Path.GetFileNameWithoutExtension(solutionFile)) + ".aar";
            if (!File.Exists(analysisResultsFileName))
            {
                ConsoleWriter.Write(ConsoleColor.Red, $"Analysis file for solution '{solutionFile}' does not exist. Did you run the analyze command?");
                return false;
            }

            try
            {
                ConsoleWriter.Write(ConsoleColor.White, $"Uploading analysis results for solution '{solutionFile}'");

                using (var client = new HttpClient())
                using (var fs = File.OpenRead(analysisResultsFileName))
                {
                    client.BaseAddress = new Uri(input.Url);

                    var body = new StreamContent(fs);
                    body.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var response = await client.PostAsync($"api/v2/products/{input.ProductName}/versions/{input.Version}/analysis/reports", body);
                    response.EnsureSuccessStatusCode();
                }

                ConsoleWriter.Write(ConsoleColor.White, $"Succesfully uploaded analysis results for solution '{solutionFile}'");
                return true;
            }
            catch (Exception ex)
            {
                ConsoleWriter.Write(ConsoleColor.Red, $"An error occured while uploading analysis results for solution '{solutionFile}'");
                ConsoleWriter.Write(ConsoleColor.White, ex.ToString());
                return false;
            }
        }
    }
}
