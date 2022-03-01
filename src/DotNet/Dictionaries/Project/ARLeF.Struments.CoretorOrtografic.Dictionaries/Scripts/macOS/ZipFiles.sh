# This script zips all the dictionaries (and radix tree) files, so that they can be stored in version control

# Elisions
cd ../../Files/Elisions/
ELISIONS=/elisions.zip
if test -f "$ELISIONS"; then
  echo "$ELISIONS archive has already been created."
else
  echo "$ELISIONS archive not yet created. Creating..."
  zip -s 10m elisions.zip elisions.db
fi
# Elisions

