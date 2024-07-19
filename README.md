# Ingestify

Ingestify can be used to streamline the process of aggregating source files from multiple subdirectories
into a single file. This makes it easier to pass the aggregated content to analysis tools or AI bots for further
processing. Ingestify also supports ignoring files and directories using patterns similar to `.gitignore`.

## Features

- Aggregate Source Files: Recursively collects and writes all files from a specified directory into a single output file.
- Ignore Patterns: Supports `.gitignore`-like ignore patterns to exclude specific files and directories from aggregation.
- Easy-to-Use CLI: Simple command-line interface for specifying input directories, output files, and ignore patterns.

## Installation

1. Download the [latest release](https://github.com/usmanmehmood55/Ingestify/releases) and use the .exe for Windows x64.
2. For other architectures, build from source.

## Build from Source

1. Clone the repository:
   ```sh
   git clone https://github.com/usmanmehmood55/Ingestify.git
   cd Ingestify
   ```

2. Build the project:
   ```sh
   dotnet build
   ```

3. Run the application:
   ```sh
   dotnet run -in <input_directory> -out <output_file> -ignore <ignore_file>
   ```

## Usage

### Command-Line Arguments

- `-in <input_directory>`: Specify the root directory to start aggregating files from.
- `-out <output_file>`: Specify the path to the output file where all aggregated content will be written.
- `-ignore <ignore_file>`: Specify the path to a file containing ignore patterns.

### Example

```sh
dotnet run -in ./src -out all_sources.txt -ignore ingestify_ignore.txt
```

In this example:
- All files in the `./src` directory and its subdirectories will be aggregated.
- The output will be written to `all_sources.txt`.
- Files and directories specified in the `ingestify_ignore.txt` file will be excluded from the aggregation.

## Configuration

### Ignore Patterns

Ingestify supports ignore patterns similar to `.gitignore`. Create a file with the patterns you want to
ignore and specify its path using the `-ignore` argument.

Example `ingestify_ignore.txt` file:
```
# Ignore all .log files
*.log

# Ignore all files in the temp directory
temp/
```

## Error Handling

Ingestify provides useful return codes and messages to help you debug issues:
- `OK`, 0: No errors.
- `UNKNOWN`, 1: Unknown error.
- `NO_INPUT_ARGS`, 2: No arguments were provided.
- `INVALID_INPUT_ARG`, 3 : An invalid argument was provided.
- `INPUT_DIR_NOT_GIVEN`, 4: The input directory was not specified.
- `INPUT_DIRECTORY_NOT_FOUND`, 5: The specified input directory does not exist.
- `OUTPUT_DIR_NOT_GIVEN`, 6: The output directory was not specified.
- `IGNORE_PATH_NOT_GIVEN`, 7: The ignore file path was not specified.
- `IGNORE_FILE_NOT_FOUND`, 8: The specified ignore file was not found.
