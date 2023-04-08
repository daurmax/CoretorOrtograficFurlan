#!/bin/bash

# This script zips all the dictionaries (and radix tree) files, so that they can be stored in version control

# Elisions - BerkeleyDB
cd ../../Dictionaries/Elisions/BerkeleyDB/
if [ -e elisions.zip ]
then
  echo "BerkeleyDB Elisions archive has already been created."
else
  echo "BerkeleyDB Elisions archive not yet created. Creating..."
  zip -s 5m elisions.zip elisions.db
fi
# Elisions - BerkeleyDB

# Elisions - SQLite
cd ../../../Dictionaries/Elisions/SQLite/
if [ -e elisions.zip ]
then
  echo "SQLite Elisions archive has already been created."
else
  echo "SQLite Elisions archive not yet created. Creating..."
  zip -s 5m elisions.zip elisions.db
fi
# Elisions - SQLite

# Errors - BerkeleyDB
cd ../../../Dictionaries/Errors/BerkeleyDB/
if [ -e errors.zip ]
then
  echo "BerkeleyDB Errors archive has already been created."
else
  echo "BerkeleyDB Errors archive not yet created. Creating..."
  zip -s 5m errors.zip errors.db
fi
# Errors - BerkeleyDB

# Errors - SQLite
cd ../../../Dictionaries/Errors/SQLite/
if [ -e errors.zip ]
then
  echo "SQLite Errors archive has already been created."
else
  echo "SQLite Errors archive not yet created. Creating..."
  zip -s 5m errors.zip errors.db
fi
# Errors - SQLite

# Frec - BerkeleyDB
cd ../../../Dictionaries/Frec/BerkeleyDB/
if [ -e frec.zip ]
then
  echo "BerkeleyDB Frec archive has already been created."
else
  echo "BerkeleyDB Frec archive not yet created. Creating..."
  zip -s 5m frec.zip frec.db
fi
# Frec - BerkeleyDB

# Frec - SQLite
cd ../../../Dictionaries/Frec/SQLite/
if [ -e frec.zip ]
then
  echo "SQLite Frec archive has already been created."
else
  echo "SQLite Frec archive not yet created. Creating..."
  zip -s 5m frec.zip frec.db
fi
# Frec - SQLite

# Words database - BerkeleyDB
cd ../../../Dictionaries/WordsDatabase/BerkeleyDB/
if [ -e words.zip ]
then
  echo "BerkeleyDB WordsDatabase archive has already been created."
else
  echo "BerkeleyDB WordsDatabase archive not yet created. Creating..."
  zip -s 5m words_split.zip words.db
fi
# Words database - BerkeleyDB

# Words database - LiteDB
cd ../../../Dictionaries/WordsDatabase/LiteDB/
if [ -e words.zip ]
then
  echo "LiteDB WordsDatabase archive has already been created."
else
  echo "LiteDB WordsDatabase archive not yet created. Creating..."
  zip -s 5m words_split.zip words.db
fi
# Words database - LiteDB

# Words database - SQLite
cd ../../../Dictionaries/WordsDatabase/SQLite/
if [ -e words.zip ]
then
  echo "SQLite WordsDatabase archive has already been created."
else
  echo "SQLite WordsDatabase archive not yet created. Creating..."
  zip -s 5m words_split.zip words.db
fi
# Words database - SQLite

# Words radix tree
cd ../../../Dictionaries/WordsRadixTree/
if [ -e words.zip ]
then
  echo "WordsRadixTree archive has already been created."
else
  echo "WordsRadixTree archive not yet created. Creating..."
  zip -s 5m words_split.zip words.rt
fi
# Words radix tree