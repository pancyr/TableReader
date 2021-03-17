using System;
using System.Collections.Generic;
using TableReader.Core;

namespace MetService
{
    [TableColumn(3, "Ед.изм")]
    [TableColumn(4, "Цена")]
    public class MetServiceCreator : ParserCreatorBase
    {
        public override TableParserBase GetTableParserObject()
        {
            return new MetServiceParser();
        }
    }
}
