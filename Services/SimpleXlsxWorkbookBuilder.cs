using System.Globalization;
using System.IO.Compression;
using System.Text;
using System.Xml.Linq;

namespace CinemaPlus.CinemaWebApp.Services;

public static class SimpleXlsxWorkbookBuilder
{
    private static readonly XNamespace SpreadsheetNamespace = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
    private static readonly XNamespace RelationshipsNamespace = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";
    private static readonly XNamespace PackageRelationshipsNamespace = "http://schemas.openxmlformats.org/package/2006/relationships";
    private static readonly XNamespace ContentTypesNamespace = "http://schemas.openxmlformats.org/package/2006/content-types";
    private static readonly XNamespace CorePropertiesNamespace = "http://schemas.openxmlformats.org/package/2006/metadata/core-properties";
    private static readonly XNamespace DublinCoreNamespace = "http://purl.org/dc/elements/1.1/";
    private static readonly XNamespace DublinCoreTermsNamespace = "http://purl.org/dc/terms/";
    private static readonly XNamespace DublinCoreTypesNamespace = "http://purl.org/dc/dcmitype/";
    private static readonly XNamespace XsiNamespace = "http://www.w3.org/2001/XMLSchema-instance";
    private static readonly XNamespace ExtendedPropertiesNamespace = "http://schemas.openxmlformats.org/officeDocument/2006/extended-properties";
    private static readonly XNamespace VariantTypesNamespace = "http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes";

    public static byte[] Build(IReadOnlyList<ExcelWorksheetData> sheets)
    {
        ArgumentNullException.ThrowIfNull(sheets);

        using var stream = new MemoryStream();
        using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
        {
            AddEntry(archive, "[Content_Types].xml", BuildContentTypesXml(sheets.Count));
            AddEntry(archive, "_rels/.rels", BuildRootRelationshipsXml());
            AddEntry(archive, "docProps/app.xml", BuildAppPropertiesXml(sheets));
            AddEntry(archive, "docProps/core.xml", BuildCorePropertiesXml());
            AddEntry(archive, "xl/workbook.xml", BuildWorkbookXml(sheets));
            AddEntry(archive, "xl/_rels/workbook.xml.rels", BuildWorkbookRelationshipsXml(sheets.Count));
            AddEntry(archive, "xl/styles.xml", BuildStylesXml());

            for (var index = 0; index < sheets.Count; index++)
            {
                AddEntry(archive, $"xl/worksheets/sheet{index + 1}.xml", BuildWorksheetXml(sheets[index]));
            }
        }

        return stream.ToArray();
    }

    private static void AddEntry(ZipArchive archive, string path, XDocument document)
    {
        var entry = archive.CreateEntry(path, CompressionLevel.Fastest);
        using var entryStream = entry.Open();
        using var writer = new StreamWriter(entryStream, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        document.Save(writer);
    }

    private static XDocument BuildContentTypesXml(int sheetCount)
    {
        var root = new XElement(ContentTypesNamespace + "Types",
            new XElement(ContentTypesNamespace + "Default",
                new XAttribute("Extension", "rels"),
                new XAttribute("ContentType", "application/vnd.openxmlformats-package.relationships+xml")),
            new XElement(ContentTypesNamespace + "Default",
                new XAttribute("Extension", "xml"),
                new XAttribute("ContentType", "application/xml")),
            new XElement(ContentTypesNamespace + "Override",
                new XAttribute("PartName", "/xl/workbook.xml"),
                new XAttribute("ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml")),
            new XElement(ContentTypesNamespace + "Override",
                new XAttribute("PartName", "/xl/styles.xml"),
                new XAttribute("ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml")),
            new XElement(ContentTypesNamespace + "Override",
                new XAttribute("PartName", "/docProps/core.xml"),
                new XAttribute("ContentType", "application/vnd.openxmlformats-package.core-properties+xml")),
            new XElement(ContentTypesNamespace + "Override",
                new XAttribute("PartName", "/docProps/app.xml"),
                new XAttribute("ContentType", "application/vnd.openxmlformats-officedocument.extended-properties+xml")));

        for (var index = 1; index <= sheetCount; index++)
        {
            root.Add(new XElement(ContentTypesNamespace + "Override",
                new XAttribute("PartName", $"/xl/worksheets/sheet{index}.xml"),
                new XAttribute("ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml")));
        }

        return new XDocument(new XDeclaration("1.0", "UTF-8", "yes"), root);
    }

    private static XDocument BuildRootRelationshipsXml()
    {
        return new XDocument(
            new XDeclaration("1.0", "UTF-8", "yes"),
            new XElement(PackageRelationshipsNamespace + "Relationships",
                new XElement(PackageRelationshipsNamespace + "Relationship",
                    new XAttribute("Id", "rId1"),
                    new XAttribute("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument"),
                    new XAttribute("Target", "xl/workbook.xml")),
                new XElement(PackageRelationshipsNamespace + "Relationship",
                    new XAttribute("Id", "rId2"),
                    new XAttribute("Type", "http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties"),
                    new XAttribute("Target", "docProps/core.xml")),
                new XElement(PackageRelationshipsNamespace + "Relationship",
                    new XAttribute("Id", "rId3"),
                    new XAttribute("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/extended-properties"),
                    new XAttribute("Target", "docProps/app.xml"))));
    }

    private static XDocument BuildAppPropertiesXml(IReadOnlyList<ExcelWorksheetData> sheets)
    {
        var titles = new XElement(VariantTypesNamespace + "vector",
            new XAttribute("size", sheets.Count),
            new XAttribute("baseType", "lpstr"));

        foreach (var sheet in sheets)
        {
            titles.Add(new XElement(VariantTypesNamespace + "lpstr", sheet.Name));
        }

        return new XDocument(
            new XDeclaration("1.0", "UTF-8", "yes"),
            new XElement(ExtendedPropertiesNamespace + "Properties",
                new XAttribute(XNamespace.Xmlns + "vt", VariantTypesNamespace),
                new XElement(ExtendedPropertiesNamespace + "Application", "CinemaPlus"),
                new XElement(ExtendedPropertiesNamespace + "DocSecurity", 0),
                new XElement(ExtendedPropertiesNamespace + "ScaleCrop", "false"),
                new XElement(ExtendedPropertiesNamespace + "HeadingPairs",
                    new XElement(VariantTypesNamespace + "vector",
                        new XAttribute("size", 2),
                        new XAttribute("baseType", "variant"),
                        new XElement(VariantTypesNamespace + "variant",
                            new XElement(VariantTypesNamespace + "lpstr", "Worksheets")),
                        new XElement(VariantTypesNamespace + "variant",
                            new XElement(VariantTypesNamespace + "i4", sheets.Count)))),
                new XElement(ExtendedPropertiesNamespace + "TitlesOfParts", titles),
                new XElement(ExtendedPropertiesNamespace + "Company", "CinemaPlus"),
                new XElement(ExtendedPropertiesNamespace + "LinksUpToDate", "false"),
                new XElement(ExtendedPropertiesNamespace + "SharedDoc", "false"),
                new XElement(ExtendedPropertiesNamespace + "HyperlinksChanged", "false"),
                new XElement(ExtendedPropertiesNamespace + "AppVersion", "1.0")));
    }

    private static XDocument BuildCorePropertiesXml()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);

        return new XDocument(
            new XDeclaration("1.0", "UTF-8", "yes"),
            new XElement(CorePropertiesNamespace + "coreProperties",
                new XAttribute(XNamespace.Xmlns + "dc", DublinCoreNamespace),
                new XAttribute(XNamespace.Xmlns + "dcterms", DublinCoreTermsNamespace),
                new XAttribute(XNamespace.Xmlns + "dcmitype", DublinCoreTypesNamespace),
                new XAttribute(XNamespace.Xmlns + "xsi", XsiNamespace),
                new XElement(DublinCoreNamespace + "creator", "CinemaPlus"),
                new XElement(CorePropertiesNamespace + "lastModifiedBy", "CinemaPlus"),
                new XElement(DublinCoreTermsNamespace + "created",
                    new XAttribute(XsiNamespace + "type", "dcterms:W3CDTF"),
                    timestamp),
                new XElement(DublinCoreTermsNamespace + "modified",
                    new XAttribute(XsiNamespace + "type", "dcterms:W3CDTF"),
                    timestamp)));
    }

    private static XDocument BuildWorkbookXml(IReadOnlyList<ExcelWorksheetData> sheets)
    {
        return new XDocument(
            new XDeclaration("1.0", "UTF-8", "yes"),
            new XElement(SpreadsheetNamespace + "workbook",
                new XAttribute(XNamespace.Xmlns + "r", RelationshipsNamespace),
                new XElement(SpreadsheetNamespace + "sheets",
                    sheets.Select((sheet, index) => new XElement(SpreadsheetNamespace + "sheet",
                        new XAttribute("name", sheet.Name),
                        new XAttribute("sheetId", index + 1),
                        new XAttribute(RelationshipsNamespace + "id", $"rId{index + 1}"))))));
    }

    private static XDocument BuildWorkbookRelationshipsXml(int sheetCount)
    {
        var root = new XElement(PackageRelationshipsNamespace + "Relationships");

        for (var index = 1; index <= sheetCount; index++)
        {
            root.Add(new XElement(PackageRelationshipsNamespace + "Relationship",
                new XAttribute("Id", $"rId{index}"),
                new XAttribute("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet"),
                new XAttribute("Target", $"worksheets/sheet{index}.xml")));
        }

        root.Add(new XElement(PackageRelationshipsNamespace + "Relationship",
            new XAttribute("Id", $"rId{sheetCount + 1}"),
            new XAttribute("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles"),
            new XAttribute("Target", "styles.xml")));

        return new XDocument(new XDeclaration("1.0", "UTF-8", "yes"), root);
    }

    private static XDocument BuildStylesXml()
    {
        return new XDocument(
            new XDeclaration("1.0", "UTF-8", "yes"),
            new XElement(SpreadsheetNamespace + "styleSheet",
                new XElement(SpreadsheetNamespace + "fonts",
                    new XAttribute("count", 1),
                    new XElement(SpreadsheetNamespace + "font",
                        new XElement(SpreadsheetNamespace + "sz", new XAttribute("val", 11)),
                        new XElement(SpreadsheetNamespace + "name", new XAttribute("val", "Calibri")))),
                new XElement(SpreadsheetNamespace + "fills",
                    new XAttribute("count", 2),
                    new XElement(SpreadsheetNamespace + "fill",
                        new XElement(SpreadsheetNamespace + "patternFill", new XAttribute("patternType", "none"))),
                    new XElement(SpreadsheetNamespace + "fill",
                        new XElement(SpreadsheetNamespace + "patternFill", new XAttribute("patternType", "gray125")))),
                new XElement(SpreadsheetNamespace + "borders",
                    new XAttribute("count", 1),
                    new XElement(SpreadsheetNamespace + "border",
                        new XElement(SpreadsheetNamespace + "left"),
                        new XElement(SpreadsheetNamespace + "right"),
                        new XElement(SpreadsheetNamespace + "top"),
                        new XElement(SpreadsheetNamespace + "bottom"),
                        new XElement(SpreadsheetNamespace + "diagonal"))),
                new XElement(SpreadsheetNamespace + "cellStyleXfs",
                    new XAttribute("count", 1),
                    new XElement(SpreadsheetNamespace + "xf",
                        new XAttribute("numFmtId", 0),
                        new XAttribute("fontId", 0),
                        new XAttribute("fillId", 0),
                        new XAttribute("borderId", 0))),
                new XElement(SpreadsheetNamespace + "cellXfs",
                    new XAttribute("count", 1),
                    new XElement(SpreadsheetNamespace + "xf",
                        new XAttribute("numFmtId", 0),
                        new XAttribute("fontId", 0),
                        new XAttribute("fillId", 0),
                        new XAttribute("borderId", 0),
                        new XAttribute("xfId", 0))),
                new XElement(SpreadsheetNamespace + "cellStyles",
                    new XAttribute("count", 1),
                    new XElement(SpreadsheetNamespace + "cellStyle",
                        new XAttribute("name", "Normal"),
                        new XAttribute("xfId", 0),
                        new XAttribute("builtinId", 0)))));
    }

    private static XDocument BuildWorksheetXml(ExcelWorksheetData sheet)
    {
        var rows = new List<XElement>();
        var rowIndex = 1;

        rows.Add(new XElement(SpreadsheetNamespace + "row",
            new XAttribute("r", rowIndex),
            sheet.Headers.Select((header, columnIndex) => BuildInlineStringCell(rowIndex, columnIndex + 1, header))));

        rowIndex++;

        foreach (var row in sheet.Rows)
        {
            rows.Add(new XElement(SpreadsheetNamespace + "row",
                new XAttribute("r", rowIndex),
                row.Select((cell, columnIndex) => BuildCell(rowIndex, columnIndex + 1, cell))));
            rowIndex++;
        }

        return new XDocument(
            new XDeclaration("1.0", "UTF-8", "yes"),
            new XElement(SpreadsheetNamespace + "worksheet",
                new XElement(SpreadsheetNamespace + "sheetData", rows)));
    }

    private static XElement BuildCell(int rowIndex, int columnIndex, object? value)
    {
        return value switch
        {
            null => BuildInlineStringCell(rowIndex, columnIndex, string.Empty),
            int intValue => BuildNumericCell(rowIndex, columnIndex, intValue.ToString(CultureInfo.InvariantCulture)),
            long longValue => BuildNumericCell(rowIndex, columnIndex, longValue.ToString(CultureInfo.InvariantCulture)),
            float floatValue => BuildNumericCell(rowIndex, columnIndex, floatValue.ToString(CultureInfo.InvariantCulture)),
            double doubleValue => BuildNumericCell(rowIndex, columnIndex, doubleValue.ToString(CultureInfo.InvariantCulture)),
            decimal decimalValue => BuildNumericCell(rowIndex, columnIndex, decimalValue.ToString(CultureInfo.InvariantCulture)),
            DateTime dateTimeValue => BuildInlineStringCell(rowIndex, columnIndex, dateTimeValue.ToString("dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture)),
            _ => BuildInlineStringCell(rowIndex, columnIndex, Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty)
        };
    }

    private static XElement BuildNumericCell(int rowIndex, int columnIndex, string value)
    {
        return new XElement(SpreadsheetNamespace + "c",
            new XAttribute("r", GetCellReference(rowIndex, columnIndex)),
            new XElement(SpreadsheetNamespace + "v", value));
    }

    private static XElement BuildInlineStringCell(int rowIndex, int columnIndex, string value)
    {
        return new XElement(SpreadsheetNamespace + "c",
            new XAttribute("r", GetCellReference(rowIndex, columnIndex)),
            new XAttribute("t", "inlineStr"),
            new XElement(SpreadsheetNamespace + "is",
                new XElement(SpreadsheetNamespace + "t", value)));
    }

    private static string GetCellReference(int rowIndex, int columnIndex)
    {
        return $"{GetColumnName(columnIndex)}{rowIndex}";
    }

    private static string GetColumnName(int columnIndex)
    {
        var builder = new StringBuilder();
        var current = columnIndex;

        while (current > 0)
        {
            current--;
            builder.Insert(0, (char)('A' + current % 26));
            current /= 26;
        }

        return builder.ToString();
    }
}

public sealed record ExcelWorksheetData(
    string Name,
    IReadOnlyList<string> Headers,
    IReadOnlyList<IReadOnlyList<object?>> Rows);
