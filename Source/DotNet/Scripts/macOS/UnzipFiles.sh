#!/bin/bash

# This script zips all the dictionaries (and radix tree) files, so that they can be stored in version control

# Elisions
cd ../../Libraries/ARLeF.Struments.CoretorOrtografic.Dictionaries/Dictionaries/Elisions/
if [ -e elisions.db ]
then
  echo "Elisions dictionary has already been extrated from archive."
else
  echo "Elisions dictionary not yet extracted. Extracting..."
  unzip elisions.zip
fi
# Elisions

# Errors
cd ../../Dictionaries/Errors/
if [ -e errors.db ]
then
  echo "Errors dictionary has already been extrated from archive."
else
  echo "Errors dictionary not yet extracted. Extracting..."
  unzip errors.zip
fi
# Errors

# Frec
cd ../../Dictionaries/Frec/
if [ -e frec.db ]
then
  echo "Frec dictionary has already been extrated from archive."
else
  echo "Frec dictionary not yet extracted. Extracting..."
  unzip frec.zip
fi
# Frec

# Words database
cd ../../Dictionaries/WordsDatabase/
if [ -e words.db ]
then
  echo "WordsDatabase dictionary has already been extrated from archive."
else
  echo "WordsDatabase dictionary not yet extracted. Extracting..."
  zip -F words_split.zip --output words.zip
  unzip words.zip
fi
# Words database

# Words radix tree
cd ../../Dictionaries/WordsRadixTree/
if [ -e words.rt ]
then
  echo "WordsRadixTree has already been extrated from archive."
else
  echo "WordsRadixTree not yet extracted. Extracting..."
  zip -F words_split.zip --output words.zip
  unzip words.zip
fi
# Words radix tree