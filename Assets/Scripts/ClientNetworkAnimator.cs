using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode.Components;

public class ClientNetworkAnimator : NetworkAnimator
{
    override protected bool OnIsServerAuthoritative()
    {
        return false;
    }
}
