using UnityEngine;

/// Shared user feedback - a one-shot particle burst plus a spatialized sound, both
/// placed at the same point so they draw attention to where something happened.
public static class Feedback
{
    /// Plays the burst and the sound at <paramref name="position"/>. Either may be null.
    /// Returns the spawned burst so callers can tweak it (e.g. tint it).
    public static ParticleSystem Play(ParticleSystem burstPrefab, AudioClip clip, Vector3 position, float minDistance = 1f)
    {
        ParticleSystem burst = null;
        if (burstPrefab != null)
        {
            // The prefab's Stop Action is Destroy, so it cleans itself up.
            burst = Object.Instantiate(burstPrefab, position, Quaternion.identity);
            burst.Play();
        }

        if (clip != null)
        {
            // A throwaway 3D AudioSource at the spot, removed once the clip ends.
            var go = new GameObject("Feedback Sound");
            go.transform.position = position;
            var source = go.AddComponent<AudioSource>();
            source.clip = clip;
            source.spatialBlend = 1f;
            source.minDistance = minDistance;
            source.maxDistance = 40f;
            source.rolloffMode = AudioRolloffMode.Logarithmic;
            source.Play();
            Object.Destroy(go, clip.length + 0.1f);
        }

        return burst;
    }
}
