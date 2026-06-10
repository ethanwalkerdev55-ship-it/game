using UnityEngine;

public static class ClientModSlashPanel
{
    public static bool Visible;

    private static bool slashKeyLatched;

    public static void TryToggleFromKeys()
    {
        bool slashDown = Input.GetKeyDown(47) || Input.GetKeyDown(63) || Input.GetKeyDown(288);
        if (slashDown)
        {
            if (slashKeyLatched)
            {
                return;
            }

            slashKeyLatched = true;
            Visible = !Visible;
            Logger.Log("[ClientMod] Slash panel " + (Visible ? "open" : "close"));
            return;
        }

        if (!Input.GetKey(47) && !Input.GetKey(63) && !Input.GetKey(288))
        {
            slashKeyLatched = false;
        }
    }

    public static void Draw()
    {
        if (!Visible)
        {
            return;
        }

        float w = 1020f;
        float h = 638f;
        float x = ((float)Screen.width - w) * 0.5f;
        float y = ((float)Screen.height - h) * 0.5f;

        GUI.depth = 100;
        GUI.Box(new Rect(x - 8f, y - 8f, w + 16f, h + 16f), "");
        GUI.Box(new Rect(x, y, w, h), "CLIENT MOD");
        GUI.Box(new Rect(x + 8f, y + 12f, 250f, 36f), "GRAPHICS & SOUND");
        GUI.Box(new Rect(x + 270f, y + 12f, 170f, 36f), "GAME UI");
        GUI.Box(new Rect(x + 450f, y + 12f, 170f, 36f), "SOCIAL");
        GUI.Box(new Rect(x + 630f, y + 12f, 170f, 36f), "CONTROLS");
        GUI.Box(new Rect(x + 10f, y + 54f, w - 20f, h - 64f), "");
    }
}
