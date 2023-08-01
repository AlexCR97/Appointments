<script lang="ts">
	import { AppHeader, PageHeader, Sidebar } from '$lib/components';

	const sidebarWidth = '250px';
</script>

<div class="border" style="width: 100vw; height: 100vh">
	<div class="d-flex w-100 h-100">
		<aside class="border-end h-100 overflow-y-auto" style:width={sidebarWidth}>
			<Sidebar />
		</aside>

		<div class="w-100 h-100 d-flex flex-column">
			<AppHeader />

			<PageHeader />

			<div class="w-100 h-100 overflow-auto">
				<main class="container py-4">
					<slot />
				</main>
			</div>
		</div>
	</div>
</div>
