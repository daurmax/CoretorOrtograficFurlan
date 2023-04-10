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
my $dir = rel2abs('../../../ARLeF.Struments.CoretorOrtografic.Dictionaries/Dictionaries/Frec/BerkeleyDB');
my $freq_file = catfile($dir, 'frec.db');
my $sqlite_file = rel2abs('../../../ARLeF.Struments.CoretorOrtografic.Dictionaries/Dictionaries/Frec/SQLite/frequencies.sqlite');
my $zip_file = rel2abs('../../../ARLeF.Struments.CoretorOrtografic.Dictionaries/Dictionaries/Frec/SQLite/frequencies.zip');

# Load the frequency hash
print "Loading frequency hash...\n";
my %freq_hash;
my $dbf = tie(%freq_hash, 'DB_File', $freq_file, O_RDONLY, 0666)
  or die("Cannot open frequency file '$freq_file': $!\n");
$dbf->filter_fetch_key(sub { utf8::decode($_) });
$dbf->filter_fetch_value(sub { $_ = unpack("C", $_) });

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
    Value INTEGER
)
SQL

# Begin a transaction to speed up the insertion process
print "Inserting frequencies into SQLite database...\n";
$dbh->begin_work;

# Insert the frequencies from the frequency hash into the SQLite database
while (my ($word, $frequency) = each %freq_hash) {
    $dbh->do("INSERT OR REPLACE INTO Data (Key, Value) VALUES (?, ?)", undef, $word, $frequency);
}

# Commit the transaction
print "Committing the transaction...\n";
$dbh->commit;

# Check if the frequencies match for a few words
print "Checking if frequencies match...\n";
my @words = ('cognossi', 'cjant', 'clap');
foreach my $word (@words) {
    my $original_freq = $freq_hash{$word} // 0;
    my $sqlite_freq = $dbh->selectrow_array("SELECT Value FROM Data WHERE Key = ?", undef, $word);
    if ($original_freq == $sqlite_freq) {
        print "Frequency for '$word' matches: $original_freq\n";
    } else {
        print "Frequency for '$word' does not match: original=$original_freq, SQLite=$sqlite_freq\n";
    }
}

# Clean up
print "Cleaning up...\n";
untie %freq_hash;
$dbh->disconnect;

# Create a zip archive
print "Creating zip archive...\n";
my $zip = Archive::Zip->new();
$zip->addFile($sqlite_file, 'frequencies.sqlite');
unless ($zip->writeToFileNamed($zip_file) == AZ_OK) {
    die("Error creating zip archive '$zip_file'\n");
}

print "Done!\n";