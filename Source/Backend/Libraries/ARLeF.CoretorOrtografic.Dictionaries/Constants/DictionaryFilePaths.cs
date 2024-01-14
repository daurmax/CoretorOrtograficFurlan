using System;
using System.IO;

namespace ARLeF.CoretorOrtografic.Dictionaries.Constants
{
    public static class DictionaryFilePaths
    {
        private static readonly string AppDataFolderPath =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        private static readonly string CoretorOrtograficFolderPath =
            Path.Combine(AppDataFolderPath, "ARLeF", "CoretorOrtograficFurlan", "Dictionaries");

        public static readonly string SQLITE_ELISIONS_DATABASE_FILE_PATH = Path.Combine(CoretorOrtograficFolderPath, "elisions.sqlite");
        public static readonly string SQLITE_ERRORS_DATABASE_FILE_PATH = Path.Combine(CoretorOrtograficFolderPath, "errors.sqlite");
        public static readonly string SQLITE_FREC_DATABASE_FILE_PATH = Path.Combine(CoretorOrtograficFolderPath, "frequencies.sqlite");
        public static readonly string SQLITE_SYSTEM_DATABASE_FILE_PATH = Path.Combine(CoretorOrtograficFolderPath, "words.db");
        public static readonly string WORDS_RADIX_TREE_FILE_PATH = Path.Combine(CoretorOrtograficFolderPath, "words.rt");

        public static readonly string SQLITE_USER_DATABASE_FILE_PATH = Path.Combine(CoretorOrtograficFolderPath, "UserDictionary", "user_dictionary.sqlite");
        public static readonly string SQLITE_USER_ERRORS_DATABASE_FILE_PATH = Path.Combine(CoretorOrtograficFolderPath, "UserErrors", "user_errors.sqlite");
    }
}