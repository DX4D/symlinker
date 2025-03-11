# SYMLINKER BY DX4D
A simple program that replaces symlinks in a directory (including subfolders) with the files that the symlink points to.

# USAGE
Just drop the build into the folder you want to convert and run symlinker.exe
Any symlinks found inside of that directory and it's subfolders will be replaced with the linked file.

# NOTES
The code detects a symlink based on the first two characters of the symlink file.
If the first two characters are not ".." the file will not be recognized as a symlink.
As such, all symlinks are expected to use relative file paths and not point to files in the same project.

# LICENSE
MIT License - use it for anything