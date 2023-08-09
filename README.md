# unity-evmgames
This package contains a profile package with many vendor imports which provide integration with EVM blockchain networks.

# Install
To install this package you need to open the package manager in your project and:

  1. Add a scoped registry with:
     - "name": "AlephVault"
     - "url": "https://unity.packages.alephvault.com"
     - "scopes": ["com.alephvault"]
  2. Look for this package: `com.alephvault.unity.ipfs`.
  3. Install it.

Also, 6 compiled assemblies are needed (e.g. into the project's Plugins/ directory) for this package to work:

  - BouncyCastle.Crypto.dll
  - Common.Logging.Core.dll
  - Common.Logging.dll
  
# Usage

# Notes
This documentation has to be updated after the big migration.
