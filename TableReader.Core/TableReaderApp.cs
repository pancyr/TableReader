using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using Microsoft.Office.Interop.Excel;

namespace TableReader.Core
{
    public sealed class TableReaderApp
    {
        private static Application _excelApp;
        public static Application GetExcelApp()
        {
            try
            {
                if (_excelApp == null)
                {
                    _excelApp = new Application(); //Пробуем получить новый экземпляр Excel  
                    _excelApp.DisplayAlerts = true;
                }
                return _excelApp;
            }
            catch
            {
                throw new Exception("Ошибка создания экземпляра MS Excel");
            }
        }

        private static DoWorkEventArgs _workerEventArgs;
        public static DoWorkEventArgs WorkerArgs
        {
            get
            {
                return _workerEventArgs;
            }
            set
            {
                _workerEventArgs = value;
            }
        }

        /*public static TableParserBase.SetProgressValueHandler _setProgressDelegate;
        public static TableParserBase.SetProgressValueHandler SetProgressDelegate
        {
            get
            {
                return _setProgressDelegate;
            }
            set
            {
                _setProgressDelegate = value;
            }
        }*/

        public static TableParserBase SelectParserObjectFromAvailableModules(Page page, string modulesPath)
        {
            string[] moduleNames = Directory.GetFiles(@modulesPath, "*.dll");
            Type baseCreatorType = typeof(ParserCreatorBase);

            foreach (string module in moduleNames)
            {
                Assembly assembly = Assembly.LoadFile(module);
                Type[] typesOfAssembly = assembly.GetTypes();
                foreach (Type type in typesOfAssembly)
                    if (type.IsSubclassOf(baseCreatorType) & !type.IsAbstract
                        & type.GetCustomAttributes(typeof(SecondaryPriceAttribute), false).Length == 0)
                    {
                        ParserCreatorBase creator = Activator.CreateInstance(type) as ParserCreatorBase;
                        if (creator.DetectIncomingPageForParserClass(page))
                            return creator.GetTableParserObject();
                    }
            }
            return null;
        }
    }
}
