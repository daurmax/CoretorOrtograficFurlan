using System;
namespace ARLeF.Struments.CoretorOrtografic.Dictionaries.Constants
{
    public static class DictionaryFilePaths
    {
        private static readonly string MyDocumentsDataFolderPath =
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public static readonly string SQLITE_ELISIONS_DATABASE_FILE_PATH = "Dictionaries/Elisions/SQLite/elisions.sqlite";
        public static readonly string SQLITE_ERRORS_DATABASE_FILE_PATH = "Dictionaries/Errors/SQLite/errors.sqlite";
        public static readonly string SQLITE_FREC_DATABASE_FILE_PATH = "Dictionaries/Frec/SQLite/frequencies.sqlite";
        public static readonly string SQLITE_SYSTEM_DATABASE_FILE_PATH = "Dictionaries/WordsDatabase/SQLite/words.db";
        public static readonly string WORDS_RADIX_TREE_FILE_PATH = "Dictionaries/WordsRadixTree/words.rt";

        public static readonly string SQLITE_USER_DATABASE_FILE_PATH = $"{MyDocumentsDataFolderPath}/ARLeF/CoretorOrtograficFurlan/UserDictionary/user_dictionary.sqlite";
        public static readonly string SQLITE_USER_ERRORS_DATABASE_FILE_PATH = $"{MyDocumentsDataFolderPath}/ARLeF/CoretorOrtograficFurlan/UserErrors/user_errors.sqlite";
    }
}
