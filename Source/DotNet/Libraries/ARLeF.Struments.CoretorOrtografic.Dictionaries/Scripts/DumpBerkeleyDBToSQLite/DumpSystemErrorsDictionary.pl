# This Perl script reads in a errors hash from a BerkeleyDB file, inserts the data into a SQLite database, 
# and then creates a zip archive containing the resulting SQLite file. It also includes a check to ensure 
# that the errors data was inserted correctly.

#!/usr/bin/perl
use strict;
use warnings;
use utf8;

use DBI;
use DB_File;
use Encode qw/encode decode/;
use File::Spec::Functions qw/rel2abs catfile/;
use Archive::Zip qw(:ERROR_CODES :CONSTANTS);

# Set the directory containing the .db files
my $dir = rel2abs('../../../ARLeF.Struments.CoretorOrtografic.Dictionaries/Dictionaries/Errors/BerkeleyDB');
my $errors_file = catfile($dir, 'errors.db');
my $sqlite_file = rel2abs('../../../ARLeF.Struments.CoretorOrtografic.Dictionaries/Dictionaries/Errors/SQLite/errors.sqlite');
my $zip_file = rel2abs('../../../ARLeF.Struments.CoretorOrtografic.Dictionaries/Dictionaries/Errors/SQLite/errors.zip');

# Load the errors hash
print "Loading errors hash...\n";
my %errors_hash;
my $dbf = tie(%errors_hash, 'DB_File', $errors_file, O_RDONLY, 0666)
  or die("Cannot open errors file '$errors_file': $!\n");
$dbf->filter_fetch_key(sub { utf8::decode($_) });
$dbf->filter_fetch_value(sub { utf8::decode($_) });

# Remove the existing SQLite database file if it exists
if (-e $sqlite_file) {
    print "Removing existing SQLite database file...\n";
    unlink $sqlite_file
      or die("Failed to remove SQLite database file '$sqlite_file': $!\n");
}

# Connect to the SQLite database
print "Connecting to SQLite database...\n";
my $dbh = DBI->connect("dbi:SQLite:dbname=$sqlite_file", "", "", {
    RaiseError => 1,
    PrintError => 0,
    AutoCommit => 1,
    sqlite_use_immediate_transaction => 0,
});

# Create the SQLite table
print "Creating SQLite table...\n";
$dbh->do(<<'SQL');
CREATE TABLE IF NOT EXISTS Data (
    Key TEXT PRIMARY KEY,
    Value TEXT
)
SQL

# Begin a transaction to speed up the insertion process
print "Inserting errors into SQLite database...\n";
$dbh->begin_work;

# Insert the errors from the errors hash into the SQLite database
while (my ($word, $correction) = each %errors_hash) {
    $dbh->do("INSERT OR REPLACE INTO Data (Key, Value) VALUES (?, ?)", undef, $word, $correction);
}

# Commit the transaction
print "Committing the transaction...\n";
$dbh->commit;

# Check if the corrections match for a few words
print "Checking if corrections match...\n";
my %words = (
    'adincuatri' => 'ad in cuatri',
    'distrade' => 'di strade',
    'in tun' => 'intun'
);

foreach my $word (keys %words) {
    my $original_correction = $errors_hash{$word} // '';
    my $sqlite_correction = $dbh->selectrow_array("SELECT Value FROM Data WHERE Key = ?", undef, $word);
    if ($original_correction eq $sqlite_correction) {
        print "Correction for '$word' matches: $original_correction\n";
    } else {
        print "Correction for '$word' does not match: original=$original_correction, SQLite=$sqlite_correction\n";
    }
}

# Clean up
print "Cleaning ip...\n";
untie %errors_hash;
$dbh->disconnect;

# Create a zip archive
print "Creating zip archive...\n";
my $zip = Archive::Zip->new();
$zip->addFile($sqlite_file, 'errors.sqlite');
unless ($zip->writeToFileNamed($zip_file) == AZ_OK) {
die("Error creating zip archive '$zip_file'\n");
}

print "Done!\n";