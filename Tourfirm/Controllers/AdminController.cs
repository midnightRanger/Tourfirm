using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Tourfirm.Domain.ViewModels;


namespace Tourfirm.Controllers;

public class AdminController : Controller
{
    string Set = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "set " : "export ";
    public async Task PostgreSqlRestore(
        string inputFile,
        string host,
        string port,
        string database,
        string user,
        string password)
    {
        string dumpCommand = $"{Set}PGPASSWORD={password}\n" +
                             $"psql -h {host} -p {port} -U {user} -d {database} -c \"select pg_terminate_backend(pid) from pg_stat_activity where datname = '{database}'\"\n" +
                             $"dropdb -h " + host + " -p " + port + " -U " + user + $" {database}\n" +
                             $"createdb -h " + host + " -p " + port + " -U " + user + $" {database}\n" +
                             "pg_restore -h " + host + " -p " + port + " -d " + database + " -U " + user + "";

//psql command disconnect database
//dropdb and createdb  remove database and create.
//pg_restore restore database with file create with pg_dump command
        dumpCommand = $"{dumpCommand} {inputFile}";

        await Execute(dumpCommand);
    }
    
    private Task Execute(string dumpCommand)
    {
        return Task.Run(() =>
        {
            string batFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}." + (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "bat" : "sh"));
            try
            {
                string batchContent = "";
                batchContent += $"{dumpCommand}";

                System.IO.File.WriteAllText(batFilePath, batchContent, Encoding.ASCII);

                ProcessStartInfo info = ProcessInfoByOS(batFilePath);

                using Process proc = Process.Start(info);


                proc.WaitForExit();
                var exit = proc.ExitCode;

                proc.Close();
            }
            catch (Exception e)
            {
                // Your exception handler here.

            }
            finally
            {
                if (System.IO.File.Exists(batFilePath)) System.IO.File.Delete(batFilePath);
            }
        });
    }
    
    private static ProcessStartInfo ProcessInfoByOS(string batFilePath)
    {
        ProcessStartInfo info;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            info = new ProcessStartInfo(batFilePath)
            {
            };
        }
        else
        {
            info = new ProcessStartInfo("sh")
            {
                Arguments = $"{batFilePath}"
            };
        }

        info.CreateNoWindow = true;
        info.UseShellExecute = false;
        info.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
        info.RedirectStandardError = true;

        return info;
    }


    [HttpGet]
    public async Task<IActionResult> PostgreSqlDump()
    {
        return View("Admin"); 
    }
    public async Task<FileResult> PostgreSqlDump(
        string outFile,
        string host,
        string port,
        string database,
        string user,
        string password = "123")
    {
        outFile = $"D:\\db.sql"; 
        string dumpCommand =
            $"pg_dump" + " -Fc" + " -h " + $"{DbConnection.Server}" + " -p " + $"{DbConnection.Port}" + " -d " + $"{DbConnection.Database}" + " -U " + $"{DbConnection.UserId}" + "";

        string batchContent = "" + dumpCommand + "  > " + "\"" + outFile + "\"" + "\n";
//        if (System.IO.File.Exists(outFile)) System.IO.File.Delete(outFile);

        await Execute(batchContent);

        byte[] fileBytes = System.IO.File.ReadAllBytes(outFile);
            string fileName = "db.sql";
        
        return File(fileBytes,System.Net.Mime.MediaTypeNames.Text.Plain, fileName );
    }
}