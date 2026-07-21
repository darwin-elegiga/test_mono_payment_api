package templates

#Volume: {
	name: string
	configMap?: {
		name: string
		defaultMode?: int
	}
	secret?: {
		secretName: string
	}
	persistentVolumeClaim?: {
		claimName: string
	}
	emptyDir?: {}
}