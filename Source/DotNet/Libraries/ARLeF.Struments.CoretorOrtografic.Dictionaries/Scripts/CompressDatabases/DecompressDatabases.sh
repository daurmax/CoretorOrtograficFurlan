#!/bin/bash

# This script zips all the dictionaries (and radix tree) files, so that they can be stored in version control
# Tested on macOS 12.4

# Elisions 
cd ../../Dictionaries/Elisions/BerkeleyDB/
if [ -e elisions.db ]
then
  echo "Elisions dictionary has already been extrated from archive."
else
  echo "Elisions dictionary not yet extracted. Extracting..."
  unzip elisions.zip
fi
# Elisions

# Errors
cd ../../../Dictionaries/Errors/BerkeleyDB/
if [ -e errors.db ]
then
  echo "Errors dictionary has already been extrated from archive."
else
  echo "Errors dictionary not yet extracted. Extracting..."
  unzip errors.zip
fi
# Errors

# Frec
cd ../../../Dictionaries/Frec/BerkeleyDB/
if [ -e frec.db ]
then
  echo "Frec dictionary has already been extrated from archive."
else
  echo "Frec dictionary not yet extracted. Extracting..."
  unzip frec.zip
fi
# Frec

# Words database - BerkeleyDB
cd ../../../Dictionaries/WordsDatabase/BerkeleyDB/
if [ -e words.db ]
then
  echo "BerkeleyDB WordsDatabase dictionary has already been extrated from archive."
else
  echo "BerkeleyDB WordsDatabase dictionary not yet extracted. Extracting..."
  zip -F words_split.zip --output words.zip
  unzip words.zip
fi
# Words database - BerkeleyDB
  
# Words database - LiteDB
cd ../../../Dictionaries/WordsDatabase/LiteDB/
if [ -e words.db ]
then
  echo "LiteDB WordsDatabase dictionary has already been extrated from archive."
else
  echo "LiteDB WordsDatabase dictionary not yet extracted. Extracting..."
  zip -F words_split.zip --output words.zip
  unzip words.zip
fi
# Words database - LiteDB

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