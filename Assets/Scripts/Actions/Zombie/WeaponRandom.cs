using Spine;
using Spine.Unity;
using Spine.Unity.AttachmentTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RandomEquip;

public class WeaponRandom : MonoBehaviour
{
    public List<EquipHook> Equippables = new List<EquipHook>();  // ��Ҫ�滻�Ĺ�������������
    public List<Sprite> Weapons;  // ���滻��ͷ��ͼƬ
    public Material SourceMaterial;   // Դ����

    private Skin equipsSkin;  // ��춸����������½�Ƥ��
    private Material cloneMaterial;
    private SkeletonDataAsset skeletonDataAsset;
    private SkeletonAnimation skeletonAnimation;

    private void Start()
    {
        skeletonAnimation = this.GetComponent<SkeletonAnimation>();
        equipsSkin = new Skin("Equips");
        // ��Ĭ�ϵ�Ƥ����ӵ��½���Ƥ����
        equipsSkin.AddAttachments(skeletonAnimation.Skeleton.Skin);

        cloneMaterial = new Material(SourceMaterial);

        skeletonAnimation.Skeleton.Skin = equipsSkin;
        RefreshSkeletonAttachments();
        skeletonDataAsset = skeletonAnimation.SkeletonDataAsset;

        foreach (var item in Equippables)
        {
            Equip(item);
        }
    }

    public void Equip(EquipHook equipHook)
    {
        var skeletonData = skeletonDataAsset.GetSkeletonData(true);
        int slotIndex = skeletonData.FindSlotIndex(equipHook.slot);
        int random = Random.Range(0, Weapons.Count);
        Sprite sprite = Weapons[random];

        var attachment = GenerateAttachmentFromEquipAsset(slotIndex, equipHook.templateSkin, equipHook.templateAttachment, sprite);
        Equip(slotIndex, equipHook.templateAttachment, attachment);
    }

    Attachment GenerateAttachmentFromEquipAsset(int slotIndex, string templateSkinName, string templateAttachmentName, Sprite sprite)
    {

        Attachment attachment;

        var skeletonData = skeletonDataAsset.GetSkeletonData(true);
        var templateSkin = skeletonData.FindSkin(templateSkinName);
        Attachment templateAttachment = templateSkin.GetAttachment(slotIndex, templateAttachmentName);
        attachment = templateAttachment.GetRemappedClone(sprite, cloneMaterial);

        return attachment;
    }

    public void Equip(int slotIndex, string attachmentName, Attachment attachment)
    {
        equipsSkin.SetAttachment(slotIndex, attachmentName, attachment);
        skeletonAnimation.Skeleton.SetSkin(equipsSkin);
        RefreshSkeletonAttachments();
    }

    void RefreshSkeletonAttachments()
    {
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.AnimationState.Apply(skeletonAnimation.Skeleton); //skeletonAnimation.Update(0);
    }
}
