#define itself 0

typedef struct Link
{
	struct Link *Source; // ������ �� ��������� �����
	struct Link *Linker; // ������ �� ����� ������
	struct Link *Target; // ������ �� �������� �����
	struct Link *FirstRefererBySource; // ������ �� ������� ������ ������ ����������� �� ��� ����� � �������� ��������� �����
	struct Link *FirstRefererByLinker; // ������ �� ������� ������ ������ ����������� �� ��� ����� � �������� ����� ������
	struct Link *FirstRefererByTarget; // ������ �� ������� ������ ������ ����������� �� ��� ����� � �������� �������� �����
	struct Link *NextSiblingRefererBySource; // ������ �� ������ �������� ������ ����������� �� ��� ����� � �������� ��������� �����
	struct Link *NextSiblingRefererByLinker; // ������ �� ������ �������� ������ ����������� �� ��� ����� � �������� ����� ������
	struct Link *NextSiblingRefererByTarget; // ������ �� ������ �������� ������ ����������� �� ��� ����� � �������� �������� �����
	struct Link *PreviousSiblingRefererBySource; // ������ �� ����� �������� ������ ����������� �� ��� ����� � �������� ��������� �����
	struct Link *PreviousSiblingRefererByLinker; // ������ �� ����� �������� ������ ����������� �� ��� ����� � �������� ����� ������
	struct Link *PreviousSiblingRefererByTarget; // ������ �� ����� �������� ������ ����������� �� ��� ����� � �������� �������� �����
	unsigned long long ReferersBySourceCount; // ���������� ������ ����������� �� ��� ����� � �������� ��������� ����� (���������� ��������� � ������)
	unsigned long long ReferersByLinkerCount; // ���������� ������ ����������� �� ��� ����� � �������� ����� ������ (���������� ��������� � ������)
	unsigned long long ReferersByTargetCount; // ���������� ������ ����������� �� ��� ����� � �������� �������� ����� (���������� ��������� � ������)
	long long Timestamp; // �� �����������
} Link;

__declspec(dllexport) Link* CreateLink(Link* source, Link* linker, Link* target);
__declspec(dllexport) Link* UpdateLink(Link* link, Link* source, Link* linker, Link* target);
__declspec(dllexport) void DeleteLink(Link* link);
__declspec(dllexport) Link* ReplaceLink(Link* link, Link* replacement);
__declspec(dllexport) Link* SearchLink(Link* source, Link* linker, Link* target);

__declspec(dllexport) unsigned long long GetLinkNumberOfReferersBySource(Link *link);
__declspec(dllexport) unsigned long long GetLinkNumberOfReferersByLinker(Link *link);
__declspec(dllexport) unsigned long long GetLinkNumberOfReferersByTarget(Link *link);

__declspec(dllexport) void WalkThroughAllReferersBySource(Link* root, void __stdcall action(Link *));
__declspec(dllexport) int WalkThroughReferersBySource(Link* root, int __stdcall func(Link *));

__declspec(dllexport) void WalkThroughAllReferersByLinker(Link* root, void __stdcall func(Link *));
__declspec(dllexport) int WalkThroughReferersByLinker(Link* root, int __stdcall func(Link *));

__declspec(dllexport) void WalkThroughAllReferersByTarget(Link* root, void __stdcall action(Link *));
__declspec(dllexport) int WalkThroughReferersByTarget(Link* root, int __stdcall func(Link *));

void AttachLinkToMarker(Link *link, Link *marker);
void DetachLinkFromMarker(Link* link, Link* marker);

void DetachLink(Link* link);