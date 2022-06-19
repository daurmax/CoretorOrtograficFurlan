#!/bin/bash

# This script dumps all BerkeleyDB databases into text files and splits them into smaller files
# Tested on Ubuntu 22.04

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
    start=$SECONDS
    db_dump -p elisions.db -f elisions.txt
    duration=$(( SECONDS - start ))
    if [ -e elisions.txt ];
    then
      echo "Elisions dictionary dumped to elisions.txt in ${duration} seconds."
  
      echo "Splitting elisions.txt..."
      start=$SECONDS
    
      mkdir -p TextFiles
      split -l 1000 elisions.txt TextFiles/elisions_split
      rm elisions.txt
      duration=$(( SECONDS - start ))
      echo "elisions.txt splitted in ${duration} seconds."
    else 
      echo "elisions.txt not found. Please make sure dump procedure went successfully."
    fi
  else
    echo "Elisions dictionary not found. Please run 'DecompressDatabases.sh' script first."
  fi
fi
# Elisions