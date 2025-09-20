# UPEvents360 - GitHub Build Ready Repository

This repository contains a ready-to-upload project skeleton for UP Events 360 app.

## How to use

1. Create a new GitHub repository named `UPEvents360`.
2. Upload the contents of this ZIP (drag & drop `Upload files` in GitHub web UI).
3. Push to `main` (if using git locally) or upload directly.
4. Go to the repository -> Actions -> select `Build UPEvents360` workflow -> Run workflow.
5. After the workflow completes, download the artifact `UPEvents360_Installer`.

Notes:
- The publish step will download `ffmpeg` automatically if it's not found in `tools/ffmpeg/`.
- The app is a skeleton/demo to control GoPro via Wi-Fi and download files; adjust code to your needs.
