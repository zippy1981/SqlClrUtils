using System.Data.SqlClient;
using Microsoft.SqlServer.Server;

public static class UtilFunctions
{
    public static void RaisError(string message, short severity = (short)0, short state = (short)1)
    {
        RaisError(message, severity, state);
    }

    /// <summary>
    /// A SQLCLR wrapper method RAISERROR().
    /// </summary>
    /// <param name="message">The message raised by RAISERROR()</param>
    /// <param name="severity">The message severity.</param>
    /// <param name="state">The message state.</param>
    /// <param name="args">These are the <c>printf()</c> style arguments used as substitution parameters for <paramref name="message"/>.</param>
    /// <seealso cref="http://stackoverflow.com/a/337792/95195">Canonical StackOverflow answer on parameterizing multi-string answers.</seealso>
    public static void RaisError(string message, short severity, short state, params object[] args)
    {
        using (var cn = new SqlConnection("context connection=true"))
        using (var cmd = cn.CreateCommand())
        {
            cn.Open();

            cmd.Parameters.AddWithValue("@msg", message);
            cmd.Parameters.AddWithValue("@severity", severity);
            cmd.Parameters.AddWithValue("@state", state);

            if (args == null)
            {
                cmd.CommandText = "RAISERROR (@msg, @severity, @state)";
            }
            else
            {
                string[] argNames = new string[args.Length];
                for (int i = 0; i < args.Length; i++)
                {
                    argNames[i] = string.Format("@arg{0}", i);
                    cmd.Parameters.AddWithValue(argNames[i], args[i]);
                }
                cmd.CommandText = string.Format("RAISERROR (@msg, @severity, @state, {0})", string.Join(", ", argNames));
            }
            SqlContext.Pipe.ExecuteAndSend(cmd);
            cn.Close();
        }
    }
}