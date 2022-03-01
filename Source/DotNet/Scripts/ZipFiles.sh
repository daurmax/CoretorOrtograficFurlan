#!/bin/bash

# This script zips all the dictionaries (and radix tree) files, so that they can be stored in version control

# Elisions
cd ../../Dictionaries/Elisions/
if [ -e elisions.zip ]
then
  echo "Elisions archive has already been created."
else
  echo "Elisions archive not yet created. Creating..."
  zip -s 5m elisions.zip elisions.db
fi
# Elisions

# Errors
cd ../../Dictionaries/Errors/
if [ -e errors.zip ]
then
  echo "Errors archive has already been created."
else
  echo "Errors archive not yet created. Creating..."
  zip -s 5m errors.zip errors.db
fi
# Errors

# Frec
cd ../../Dictionaries/Frec/
if [ -e frec.zip ]
then
  echo "Frec archive has already been created."
else
  echo "Frec archive not yet created. Creating..."
  zip -s 5m frec.zip frec.db
fi
# Frec

# Words database
cd ../../Dictionaries/WordsDatabase/
if [ -e words.zip ]
then
  echo "WordsDatabase archive has already been created."
else
  echo "WordsDatabase archive not yet created. Creating..."
  zip -s 5m words_split.zip words.db
fi
# Words database

# Words radix tree
cd ../../Dictionaries/WordsRadixTree/
if [ -e words.zip ]
then
  echo "WordsRadixTree archive has already been created."
else
  echo "WordsRadixTree archive not yet created. Creating..."
  zip -s 5m words_split.zip words.rt
fi
# Words radix tree