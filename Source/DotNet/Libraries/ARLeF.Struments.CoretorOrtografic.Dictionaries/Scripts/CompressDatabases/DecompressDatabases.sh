#!/bin/bash

# This script extracts all the dictionaries (and radix tree) files from the archives

# Elisions - BerkeleyDB
cd ../../Dictionaries/Elisions/BerkeleyDB/
if [ -e elisions.db ]
then
  echo "BerkeleyDB Elisions dictionary has already been extracted from archive."
else
  echo "BerkeleyDB Elisions dictionary not yet extracted. Extracting..."
  unzip elisions.zip
fi

# Elisions - SQLite
cd ../../../Dictionaries/Elisions/SQLite/
if [ -e elisions.db ]
then
  echo "SQLite Elisions dictionary has already been extracted from archive."
else
  echo "SQLite Elisions dictionary not yet extracted. Extracting..."
  zip -F elisions_split.zip --output elisions.zip
  unzip elisions.zip
fi

# Errors - BerkeleyDB
cd ../../../Dictionaries/Errors/BerkeleyDB/
if [ -e errors.db ]
then
  echo "BerkeleyDB Errors dictionary has already been extracted from archive."
else
  echo "BerkeleyDB Errors dictionary not yet extracted. Extracting..."
  unzip errors.zip
fi

# Errors - SQLite
cd ../../../Dictionaries/Errors/SQLite/
if [ -e errors.db ]
then
  echo "SQLite Errors dictionary has already been extracted from archive."
else
  echo "SQLite Errors dictionary not yet extracted. Extracting..."
  zip -F errors_split.zip --output errors.zip
  unzip errors.zip
fi

# Frec - BerkeleyDB
cd ../../../Dictionaries/Frec/BerkeleyDB/
if [ -e frec.db ]
then
  echo "BerkeleyDB Frec dictionary has already been extracted from archive."
else
  echo "BerkeleyDB Frec dictionary not yet extracted. Extracting..."
  unzip frec.zip
fi

# Frec - SQLite
cd ../../../Dictionaries/Frec/SQLite/
if [ -e frec.db ]
then
  echo "SQLite Frec dictionary has already been extracted from archive."
else
  echo "SQLite Frec dictionary not yet extracted. Extracting..."
  zip -F frec_split.zip --output frec.zip
  unzip frec.zip
fi

# Words database - BerkeleyDB
cd ../../../Dictionaries/WordsDatabase/BerkeleyDB/
if [ -e words.db ]
then
  echo "BerkeleyDB WordsDatabase dictionary has already been extracted from archive."
else
  echo "BerkeleyDB WordsDatabase dictionary not yet extracted. Extracting..."
  zip -F words_split.zip --output words.zip
  unzip words.zip
fi

# Words database - SQLite
cd ../../../Dictionaries/WordsDatabase/SQLite/
if [ -e words.db ]
then
  echo "SQLite WordsDatabase dictionary has already been extrated from archive."
else
  echo "SQLite WordsDatabase dictionary not yet extracted. Extracting..."
  zip -F words_split.zip --output words.zip
  unzip words.zip
fi
# Words database - SQLite

# Words radix tree
cd ../../../Dictionaries/WordsRadixTree/
if [ -e words.rt ]
then
  echo "WordsRadixTree has already been extrated from archive."
else
  echo "WordsRadixTree not yet extracted. Extracting..."
  zip -F words_split.zip --output words.zip
  unzip words.zip
fi
# Words radix tree