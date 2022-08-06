namespace ETLService.Services.Templates;

public static class OutputTemplates
{
    public static string MetaLogTemplate =
        "parsed_files: {0}\n" +
        "parsed_lines: {1}\n" +
        "found_errors: {2}\n" +
        "invalid_files: [{3}]\n";
}