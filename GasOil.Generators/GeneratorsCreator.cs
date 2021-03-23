using TableReader.Core;

namespace GasOil.Generators
{
    [TableColumn(1, "id")]
    [TableColumn(2, "categ")]
    [TableColumn(3, "artikul")]
    [TableColumn(4, "producing-country")]
    [TableColumn(5, "proizvoditel")]
    public class GeneratorsCreator : ParserCreatorBase
    {
        public override TableParserBase GetTableParserObject()
        {
            return new GeneratorsParser();
        }
    }
}
