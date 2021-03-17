using TableReader.Core;

namespace GasOilService
{
    [TableColumn(1, "Категория")]
    [TableColumn(3, "Цена")]
    [TableColumn(5, "Описание")]
    [TableColumn(7, "Страна")]
    public class GasOilServiceCreator : ParserCreatorBase
    {
        public override TableParserBase GetTableParserObject()
        {
            return new GasOilServiceParser();
        }
    }
}
