import os
import platform
import subprocess
import concurrent.futures

def get_appdata_path():
    if platform.system() == 'Windows':
        return os.path.join(os.getenv('APPDATA'), "CoretorOrtograficFurlan")
    elif platform.system() == 'Darwin':
        return os.path.expanduser('~/Library/Application Support/CoretorOrtograficFurlan')
    else:
        raise Exception("Unsupported OS")

def unzip_file(zip_path, destination_folder):
    # Ensure destination folder exists
    if not os.path.exists(destination_folder):
        os.makedirs(destination_folder)

    # Path to 7-Zip executable
    seven_zip_path = r"C:\Program Files\7-Zip\7z.exe"

    # Check if 7-Zip is installed
    if not os.path.isfile(seven_zip_path):
        print("7-Zip is not installed. Please install 7-Zip to continue.")
        return

    # Use 7-Zip for Windows
    subprocess.run([seven_zip_path, "x", "-o" + destination_folder, zip_path])
    print(f"Extracted {zip_path} to {destination_folder}")

if __name__ == "__main__":
    appdata_folder = get_appdata_path()
    base_path = os.path.join(os.getcwd(), "..", "..", "Dictionaries")  # Adjust path to Dictionaries folder

    # List of zip files, including split zip files
    zip_files = [
        os.path.join(base_path, "Elisions", "SQLite", "elisions.zip"),
        os.path.join(base_path, "Errors", "SQLite", "errors.zip"),
        os.path.join(base_path, "Frec", "SQLite", "frequencies.zip"),
        os.path.join(base_path, "WordsDatabase", "SQLite", "words_split.zip"),  # For split zip
        os.path.join(base_path, "WordsRadixTree", "words_split.zip")  # For split zip
    ]

    with concurrent.futures.ThreadPoolExecutor() as executor:
        futures = [executor.submit(unzip_file, zip_file, appdata_folder) for zip_file in zip_files]
        for future in concurrent.futures.as_completed(futures):
            future.result()

    print(f"All files extracted to {appdata_folder}")