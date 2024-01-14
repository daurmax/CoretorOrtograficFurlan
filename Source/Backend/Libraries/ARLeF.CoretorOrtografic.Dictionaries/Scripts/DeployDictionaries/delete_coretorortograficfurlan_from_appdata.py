import os
import platform
import shutil

def get_appdata_path():
    if platform.system() == 'Windows':
        return os.path.join(os.getenv('APPDATA'), "CoretorOrtograficFurlan")
    elif platform.system() == 'Darwin':
        return os.path.expanduser('~/Library/Application Support/CoretorOrtograficFurlan')
    else:
        raise Exception("Unsupported OS")

def delete_folder(folder_path):
    if os.path.exists(folder_path):
        shutil.rmtree(folder_path)
        print(f"Deleted folder: {folder_path}")
    else:
        print(f"Folder not found: {folder_path}")

if __name__ == "__main__":
    appdata_folder = get_appdata_path()

    delete_folder(appdata_folder)