#!/bin/bash

# This script dumps all BerkeleyDB databases into text files and splits them into smaller files
# Tested on Ubuntu 22.04

echo "----------"
# Elisions 
DB_PATH='../../Dictionaries/Elisions/'
cd $(echo $DB_PATH)
if [ -d $(echo $DB_PATH/TextFiles/) ];
then 
  echo "Elisions database already dumped."
else
  echo "Elisions database not yet dumped. Dumping..."
  if [ -e elisions.db ];
  then
    start_time=$(date +%s.%3N)
    db_dump -p elisions.db -f elisions.txt
    end_time=$(date +%s.%3N)
    duration=$(echo "scale=3; $end_time - $start_time" | bc)
    if [ -e elisions.txt ];
    then
      echo "Elisions dictionary dumped to elisions.txt in ${duration} seconds."
  
      echo "Splitting elisions.txt..."
      start_time=$(date +%s.%3N)
    
      mkdir -p TextFiles
      split -l 1000 elisions.txt TextFiles/elisions_split
      rm elisions.txt
      end_time=$(date +%s.%3N)
      duration=$(echo "scale=3; $end_time - $start_time" | bc)
      echo "elisions.txt splitted in ${duration} seconds."
    else 
      echo "elisions.txt not found. Please make sure dump procedure went successfully."
    fi
  else
    echo "Elisions dictionary not found. Please run 'DecompressDatabases.sh' script first."
  fi
fi
echo "----------"
# Elisions

# Errors 
DB_PATH='../../Dictionaries/Errors/'
cd $(echo $DB_PATH)
if [ -d $(echo $DB_PATH/TextFiles/) ];
then 
  echo "Errors database already dumped."
else
  echo "Errors database not yet dumped. Dumping..."
  if [ -e errors.db ];
  then
    start_time=$(date +%s.%3N)
    db_dump -p errors.db -f errors.txt
    end_time=$(date +%s.%3N)
    duration=$(echo "scale=3; $end_time - $start_time" | bc)
    if [ -e errors.txt ];
    then
      echo "Errors dictionary dumped to errors.txt in ${duration} seconds."
  
      echo "Splitting errors.txt..."
      start_time=$(date +%s.%3N)
    
      mkdir -p TextFiles
      split -l 1000 errors.txt TextFiles/errors_split
      rm errors.txt
      end_time=$(date +%s.%3N)
      duration=$(echo "scale=3; $end_time - $start_time" | bc)
      echo "errors.txt splitted in ${duration} seconds."
    else 
      echo "errors.txt not found. Please make sure dump procedure went successfully."
    fi
  else
    echo "Errors dictionary not found. Please run 'DecompressDatabases.sh' script first."
  fi
fi
echo "----------"
# Errors

# Frec 
DB_PATH='../../Dictionaries/Frec/'
cd $(echo $DB_PATH)
if [ -d $(echo $DB_PATH/TextFiles/) ];
then 
  echo "Frec database already dumped."
else
  echo "Frec database not yet dumped. Dumping..."
  if [ -e frec.db ];
  then
    start_time=$(date +%s.%3N)
    db_dump -p frec.db -f frec.txt
    end_time=$(date +%s.%3N)
    duration=$(echo "scale=3; $end_time - $start_time" | bc)
    if [ -e frec.txt ];
    then
      echo "Frec dictionary dumped to frec.txt in ${duration} seconds."
  
      echo "Splitting frec.txt..."
      start_time=$(date +%s.%3N)
    
      mkdir -p TextFiles
      split -l 1000 frec.txt TextFiles/frec_split
      rm frec.txt
      end_time=$(date +%s.%3N)
      duration=$(echo "scale=3; $end_time - $start_time" | bc)
      echo "frec.txt splitted in ${duration} seconds."
    else 
      echo "frec.txt not found. Please make sure dump procedure went successfully."
    fi
  else
    echo "Frec dictionary not found. Please run 'DecompressDatabases.sh' script first."
  fi
fi
echo "----------"
# Frec

# Words 
DB_PATH='../../Dictionaries/WordsDatabase/'
cd $(echo $DB_PATH)
if [ -d $(echo $DB_PATH/TextFiles/) ];
then 
  echo "Words database already dumped."
else
  echo "Words database not yet dumped. Dumping..."
  if [ -e words.db ];
  then
    start_time=$(date +%s.%3N)
    db_dump -p words.db -f words.txt
    end_time=$(date +%s.%3N)
    duration=$(echo "scale=3; $end_time - $start_time" | bc)
    if [ -e words.txt ];
    then
      echo "Words dictionary dumped to words.txt in ${duration} seconds."
  
      echo "Splitting words.txt..."
      start_time=$(date +%s.%3N)
    
      mkdir -p TextFiles
      split -l 10000 words.txt TextFiles/words_split 
      rm words.txt
      end_time=$(date +%s.%3N)
      duration=$(echo "scale=3; $end_time - $start_time" | bc)
      echo "words.txt splitted in ${duration} seconds."
    else 
      echo "words.txt not found. Please make sure dump procedure went successfully."
    fi
  else
    echo "Words dictionary not found. Please run 'DecompressDatabases.sh' script first."
  fi
fi
echo "----------"
# Words