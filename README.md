# Mr.G-AI-Client-.Net-Library

## Build on GitHub

Open [Actions](https://github.com/RazvanManolache/Mr.G-AI-Client-.Net-Library/actions/workflows/build.yml), choose **Run workflow**, and select the branch. Builds also run on pushes and pull requests. Open a successful run and download its **Artifacts** (GitHub sign-in required; retained for 30 days).

The download contains a `.nupkg` with the .NET Standard 2.0 library and SHA-256 checksums. CI versions use `1.0.0-ci.<run number>`. Add the extracted download directory as a local NuGet source to consume it. Packages are build artifacts, not automatically published to NuGet.org.
