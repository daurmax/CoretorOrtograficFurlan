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
my $dir = rel2abs('../../../ARLeF.CoretorOrtografic.Dictionaries/Dictionaries/Elisions/BerkeleyDB');
my $elisions_file = catfile($dir, 'elisions.db');
my $sqlite_file = rel2abs('../../../ARLeF.CoretorOrtografic.Dictionaries/Dictionaries/Elisions/SQLite/elisions.sqlite');
my $zip_file = rel2abs('../../../ARLeF.CoretorOrtografic.Dictionaries/Dictionaries/Elisions/SQLite/elisions.zip');

# Load the elisions hash
print "Loading elisions hash...\n";
my %elisions_hash;
my $dbf = tie(%elisions_hash, 'DB_File', $elisions_file, O_RDONLY, 0666)
  or die("Cannot open elisions file '$elisions_file': $!\n");
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
    Word TEXT PRIMARY KEY
)
SQL

# Begin a transaction to speed up the insertion process
print "Inserting elisions into SQLite database...\n";
$dbh->begin_work;

# Insert the elisions from the elisions hash into the SQLite database
while (my ($word) = each %elisions_hash) {
    $dbh->do("INSERT OR REPLACE INTO Data (Word) VALUES (?)", undef, $word);
}

# Commit the transaction
print "Committing the transaction...\n";
$dbh->commit;

# Check if the words exist in the elisions database
print "Checking if words exist in the elisions database...\n";
my @words = ('analfabetementri', 'antagonisim', 'onomatopeichementri');
foreach my $word (@words) {
    my $is_present = $dbh->selectrow_array("SELECT EXISTS(SELECT 1 FROM Data WHERE Word = ?)", undef, $word);
    if ($is_present) {
        print "'$word' exists in the elisions database.\n";
    } else {s
        print "'$word' does not exist in the elisions database.\n";
    }
}

# Clean up
print "Cleaning up...\n";
untie %elisions_hash;
$dbh->disconnect;

# Create a zip archive
print "Creating zip archive...\n";
my $zip = Archive::Zip->new();
$zip->addFile($sqlite_file, 'elisions.sqlite');
unless ($zip->writeToFileNamed($zip_file) == AZ_OK) {
    die("Error creating zip archive '$zip_file'\n");
}

print "Done!\n";