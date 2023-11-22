
Project Requirements:

Python 3.9.18:
Make sure you have Python 3.9.18 installed. Python 3.12.0 doesn't work for installing the surprise library.
---------------------------------------------------------------------------
Install Flask using pip: (Flask 3.0.0)

Run the following command:
pip install Flask

If you're using Python 3, you might need to use pip3 instead:
pip3 install Flask

Verify the installation:
flask --version

---------------------------------------------------------------------------

numpy:
To install numpy, run the following commands:
pip install numpy
pip install numpy==1.26.2

To verify the installation, use the following Python script:
import numpy as np
print(np.__version__)

---------------------------------------------------------------------------

Surprise:
Install Homebrew:
/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/master/install.sh)"

Tap into Microsoft's repository:
brew tap microsoft/mssql-release https://github.com/Microsoft/homebrew-mssql-release

Update Homebrew:
brew update

---------------------------------------------------------------------------

Install MSODBC and MSSQL tools:

/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/master/install.sh)"
brew tap microsoft/mssql-release https://github.com/Microsoft/homebrew-mssql-release
brew update
HOMEBREW_ACCEPT_EULA=Y brew install msodbcsql17 mssql-tools



For more detailed instructions, refer to:
https://learn.microsoft.com/en-us/sql/connect/odbc/linux-mac/install-microsoft-odbc-driver-sql-server-macos?view=sql-server-ver16#17

If you encounter issues with pyodbc, you can uninstall and reinstall it. Using pip, the process would be:
pip uninstall pyodbc
pip install --no-binary :all: pyodbc

For additional troubleshooting, you can check:
https://stackoverflow.com/questions/66731036/unable-to-import-pyodbc-on-apple-silicon-symbol-not-found-sqlallochandle

---------------------------------------------------------------------------

Finally you can run the Python API by command:
Python3 RecommendationAPI.py

