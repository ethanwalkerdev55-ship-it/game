ClientFile = INBOX ONLY (files from customer)

  Drop new bundles here (e.g. main 2.unity3d).
  Do NOT extract or decompile in this folder.

  Run:  tools\FFPatch\ingest-client-bundle.bat

  That copies the bundle to the game offline cache, extracts DLLs under
  6877b37c-e9cd-4826-b82c-5e8d3d5db744\main\, syncs _inspect_bundle\client\,
  decompiles to mods\decompiled\, and updates the launcher manifest.

  Old files are moved to ClientFile\_archive\

UDP logs: _inspect_udp_listener\
