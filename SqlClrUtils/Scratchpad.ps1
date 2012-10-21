# Create the stored proc signature:
@"
IF EXISTS(SELECT name from sys.procedures WHERE name LIKE 'usp_foo')
BEGIN;
	DROP PROC usp_foo;
END;
GO
"@ +
"`r`n`r`nCREATE PROCEDURE usp_foo`r`n`t@msg_str VARCHAR(MAX),`r`n" + [string]::Join(",`r`n", (1..2099|%{ "`t@param_$($_) VARCHAR(MAX) = NULL" })) + "`r`nAS;`r`nGO"|Out-Clipboard

# Create the C# method signature
"public static string StringFormat(`r`n`t`tstring args,`r`n" +
	[string]::Join(",`r`n", (1..2099|%{ "`t`tstring param$($_) = null" })) + ")" +
@" 
`r`n`t{
`t`treturn String.Format(args,
"@ +
	[string]::Join(",`r`n", (1..2099|%{ "`t`t`tparam$($_)"})) +
");`r`n`t}" | Out-Clipboard

